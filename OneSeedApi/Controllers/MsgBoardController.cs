using LibFrame.DBModel;
using LibFrame.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using OneSeedApi.Common;
using OneSeedApi.Model;

namespace OneSeedApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MsgBoardController : ControllerBase
    {
        private readonly MessageBoardService _messageBoardService;
        public MsgBoardController(MessageBoardService messageBoardService)
        {
            _messageBoardService= messageBoardService;
        }
        [HttpGet]
        public PageDataModel<CommentsResult> Get(int pageIndex, int pageSize)
        {
            int total=0;
            List<SysMessageBoard> comments= _messageBoardService.GetMessages(pageIndex,pageSize,ref total);
            IEnumerable<CommentsResult> commentsResults= comments.Select<SysMessageBoard, CommentsResult>(c =>
            {
                return new CommentsResult()
                {
                    Id = c.ID,
                    Content = c.Content,
                    CreateTime = c.CreateTime,
                    Like = c.Like,
                    Dislike = c.DisLike,
                    Uid = c.User?.ID??0,
                    Uhead = c.User?.HeadImg,
                    Uname = c.User?.Name,
                };
            });
            PageDataModel<CommentsResult> result = new PageDataModel<CommentsResult>()
            {
                Datas = commentsResults,
                Total = total,
            };
            return result;
        }

        [HttpPost]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public ActionResult<int> Post(AddMsgBoardModel model)
        {
            if (string.IsNullOrEmpty(model.Content)) return new BadRequestObjectResult("评论内容不能为空！");
            string uid = HttpContext.GetUidByToken();
            int mid= _messageBoardService.AddMessage(int.Parse(uid), model.Content, model.ReplyId);
            return Ok(mid);
        }
    }
}
