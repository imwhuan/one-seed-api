using LibFrame.DBModel;
using LibFrame.DTOModel;
using SqlSugar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibFrame.Services
{
    public class NoteService
    {
        private readonly SqlSugarScope _scope;
        private readonly UserService _userService;
        public NoteService(SqlSugarScope scope, UserService userService)
        {
            _scope = scope;
            _userService = userService;
        }

        public int AddNote(int uid,string title,string content,string[] tags)
        {
            if (_userService.GetTblUser(uid) == null) throw new Exception("用户不存在！");
            TblNote tblNote = new TblNote()
            {
                Title = title,
                UID = uid,
                Content = content,
                CreateTime = DateTime.Now,
                Tags = tags
            };
            return _scope.Insertable(tblNote).ExecuteReturnIdentity();
        }
        public TblNote? GetTblNote(int id)
        {
            return _scope.Queryable<TblNote>().InSingle(id);
        }

        public List<NoteTitleModel> GetUserNoteList(int uid)
        {
            return _scope.Queryable<TblNote>().Select<NoteTitleModel>().Where(n=>n.UID==uid).ToList();
        }
    }
}
