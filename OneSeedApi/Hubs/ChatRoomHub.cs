using LibFrame.DBModel;
using LibFrame.Services;
using Microsoft.AspNetCore.SignalR;
using OneSeedApi.Model;
using ServiceStack.Redis;

namespace OneSeedApi.Hubs
{
    public class ChatRoomHub:Hub
    {
        private readonly IRedisClient redisClient;
        private readonly UserService userService;
        public ChatRoomHub(IRedisClient _redisClient, UserService _userService)
        {
            redisClient= _redisClient;
            userService= _userService;
        }

        public override async Task OnConnectedAsync()
        {
            Microsoft.AspNetCore.Http.HttpContext? url = Context.GetHttpContext();
            //判断建立连接时是否传入对象编号
            if (url !=null && url.Request.Query.ContainsKey("roomid") && url.Request.Query.ContainsKey("uid"))
            {
                string roomid = url.Request.Query["roomid"][0];
                string uid= url.Request.Query["uid"][0];
                if(await EnterTheRoom(roomid, int.Parse(uid)))
                {
                    await base.OnConnectedAsync();
                }
            }
        }
        public override async Task OnDisconnectedAsync(Exception? exception)
        {
            await LeaveTheRoom();
            await base.OnDisconnectedAsync(exception);
        }

        private async Task<bool> EnterTheRoom(string roomid,int uid)
        {
            TblUser? tblUser = userService.GetTblUser(uid);
            if(tblUser == null)
            {
                await SendSysMsgToCaller($"用户{uid}不存在！已将你连接断开！。");
                Context.Abort();
                return true;
            }
            else
            {
                ChatUserRedisModel model = new ChatUserRedisModel()
                {
                    roomid = roomid,
                    uid = uid,
                    uhead = tblUser.HeadImg ?? "",
                    uname = tblUser.Name ?? "",
                };
                //redis保存当前用户在哪个房间
                redisClient.Set<ChatUserRedisModel>(Context.ConnectionId, model);
                await Groups.AddToGroupAsync(Context.ConnectionId, roomid);
                ChatUserRedisModel msgModel = new()
                {
                    uname = "系统",
                    time = DateTime.Now,
                    roomid = roomid,
                    msg = $"{model.uname}~进入房间",
                };
                await SendChatMsg(msgModel);
                return true;
            }
        }
        private async Task LeaveTheRoom()
        {
            ChatUserRedisModel model= redisClient.Get<ChatUserRedisModel>(Context.ConnectionId);
            if (model != null)
            {
                ChatUserRedisModel msgModel = new()
                {
                    uname = "系统",
                    msg = $"{model.uname}~离开房间",
                    roomid = model.roomid,
                    time = DateTime.Now,
                };
                await SendChatMsg(msgModel);
                //用户离开房间删除数据
                redisClient.Remove(Context.ConnectionId);
            }
        }
        public async Task SendMsg(string msg)
        {
            ChatUserRedisModel? model = await GetChatUser();
            if (model != null)
            {
                model.msg= msg;
                model.time = DateTime.Now;
                await SendChatMsg(model);
            }
        }
        private async Task<ChatUserRedisModel?> GetChatUser()
        {
            ChatUserRedisModel model = redisClient.Get<ChatUserRedisModel>(Context.ConnectionId);
            if (model == null)
            {
                await SendSysMsgToCaller("由于系统数据异常，已将你断开连接！请重新尝试建立连接使用。");
                Context.Abort();
            }
            return model;
        }
        private async Task SendSysMsgToCaller(string msg)
        {
            ChatUserRedisModel msgModel = new()
            {
                uname = "系统",
                time = DateTime.Now,
                msg = msg,
            };
            await Clients.Caller.SendAsync("RoomSysMsg", msgModel);
        }
        private async Task SendChatMsg(ChatUserRedisModel model)
        {
            await Clients.Group(model.roomid).SendAsync("RoomSysMsg", model);
        }
    }
}
