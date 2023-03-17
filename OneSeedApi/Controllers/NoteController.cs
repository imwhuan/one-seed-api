using LibFrame.DBModel;
using LibFrame.DTOModel;
using LibFrame.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using OneSeedApi.Common;
using OneSeedApi.Model;

namespace OneSeedApi.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class NoteController : ControllerBase
    {
        private readonly NoteService _noteService;
        private readonly UserService _userService;
        public NoteController(NoteService noteService, UserService userService)
        {
            _noteService = noteService;
            _userService = userService;
        }
        [HttpPost]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public ActionResult AddNote(AddNoteModel model)
        {
            if (string.IsNullOrEmpty(model.Title)) return new BadRequestObjectResult("标题不能为空！");
            if (string.IsNullOrEmpty(model.Content)) return new BadRequestObjectResult("内容不能为空！");
            string uid = HttpContext.GetUidByToken();
            int rid= _noteService.AddNote(int.Parse(uid), model.Title, model.Content, model.Tags);
            return Ok(rid);
        }
        [HttpGet]
        public ActionResult<TblNote> GetNote(int id)
        {
            //string uid = HttpContext.GetUidByToken();
            //TblUser? tblUser = _userService.GetTblUser(int.Parse(uid));
            //if (tblUser == null) return new UnauthorizedResult();
            TblNote? note = _noteService.GetTblNote(id);
            if(note == null)
            {
                return NotFound("未找到笔记");
            }
            else
            {
                return Ok(note);
            }
        }

        [HttpGet]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public List<NoteTitleModel> GetUserNoteList()
        {
            string uid = HttpContext.GetUidByToken();
            TblUser? tblUser = _userService.GetTblUser(int.Parse(uid));
            if (tblUser == null) throw new Exception($"用户{uid}不存在！");
            List<NoteTitleModel> notes = _noteService.GetUserNoteList(tblUser.ID);
            return notes;
        }
    }
}
