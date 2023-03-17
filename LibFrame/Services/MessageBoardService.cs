using LibFrame.DBModel;
using LibFrame.DTOModel;
using LibFrame.Enums;
using SqlSugar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibFrame.Services
{
    public class MessageBoardService
    {
        private readonly SqlSugarScope _scope;
        private readonly UserService _userService;
        public MessageBoardService(SqlSugarScope scope, UserService userService)
        {
            _scope = scope;
            _userService = userService;
        }
        public List<SysMessageBoard> GetMessages(int pageIndex, int pageSize, ref int total)
        {
            List<SysMessageBoard> comments = _scope.Queryable<SysMessageBoard>().Includes(c => c.User)
                .OrderBy(c => c.ID).ToPageList(pageIndex, pageSize, ref total).ToList();
            return comments;
        }
        public SysMessageBoard? GetMessage(int id)
        {
            return _scope.Queryable<SysMessageBoard>().InSingle(id);
        }

        public int AddMessage(int uid,string content, int replyid = 0)
        {
            if (_userService.GetTblUser(uid) == null) throw new Exception("用户不存在！");
            SysMessageBoard sysMessage = new SysMessageBoard()
            {
                Uid = uid,
                Status = (int)SysStatusEnum.Normal,
                Content = content,
                CreateTime = DateTime.Now,
                Like = 0,
                DisLike = 0,
            };
            if(replyid > 0)
            {
                SysMessageBoard? LastSysMessage = GetMessage(replyid);
                if (LastSysMessage == null) throw new Exception($"回复的消息不存在！{replyid}");
                if (LastSysMessage.Status == (int)SysStatusEnum.Normal)
                {
                    sysMessage.GroupId = LastSysMessage.GroupId;
                }
                else if (Enum.TryParse<SysStatusEnum>(LastSysMessage.Status.ToString(), out SysStatusEnum res))
                {
                    throw new Exception($"该评论已 {res.GetRemark()},无法回复！{replyid}");
                }
                else
                {
                    throw new Exception($"该评论状态{LastSysMessage.Status}已不允许回复！{replyid}");
                }
            }
            else
            {
                sysMessage.GroupId = 0;
            }
            return _scope.Insertable(sysMessage).ExecuteCommand();
        }
    }
}
