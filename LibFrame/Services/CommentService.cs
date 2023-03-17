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
    internal class CommentService
    {
        private readonly SqlSugarScope _scope;
        private readonly UserService _userService;
        public CommentService(SqlSugarScope scope, UserService userService)
        {
            _scope = scope;
            _userService = userService;
        }

        public List<TblComment> GetComments(int pageIndex, int pageSize, ref int total)
        {
            List<TblComment> comments = _scope.Queryable<TblComment>()
                .Where(c => c.MediaType == (int)SysMediaContentTypeEnum.MessageBoard).Includes(c => c.User)
                .OrderBy(c => c.ID).ToPageList(pageIndex, pageSize, ref total).ToList();
            return comments;
        }
        public List<TblSubComment> GetSubComments(int pid, int pageIndex, int pageSize, ref int total)
        {
            List<TblSubComment> comments = _scope.Queryable<TblSubComment>()
                .Where(c => c.GroupId == pid).ToPageList(pageIndex, pageSize, ref total).ToList();
            return comments;
        }

        public int AddComment(CommentAddModel comment)
        {
            if (_userService.GetTblUser(comment.Uid) == null) throw new Exception("用户不存在！");
            TblComment newComment = new TblComment()
            {
                Status = (int)SysStatusEnum.Normal,
                CreateTime = DateTime.Now,
                MediaId = comment.MediaId,
                MediaType = (int)comment.MediaType,
                Content = comment.Content,
                Uid = comment.Uid,
            };
            return _scope.Insertable(newComment).ExecuteCommand();
        }
        public int AddSubComment(CommentAddModel comment)
        {
            if (_userService.GetTblUser(comment.Uid) == null) throw new Exception("用户不存在！");
            TblSubComment newComment = new TblSubComment()
            {
                ReplyTo = comment.ReplyTo,
                Status = (int)SysStatusEnum.Normal,
                CreateTime = DateTime.Now,
                Content = comment.Content,
                Uid = comment.Uid,
                GroupId = comment.GroupId,
            };
            int LastCommentStatus;
            if (comment.GroupId == comment.ReplyTo)
            {
                //层主是顶级回复
                TblComment? LastComment = GetComment(comment.ReplyTo);
                if (LastComment == null)
                {
                    throw new Exception("回复对象不存在！");
                }
                else
                {
                    LastCommentStatus = LastComment.Status;
                }
            }
            else
            {
                //层主是二级回复或者三四五级等回复
                TblSubComment? LastComment = GetSubComment(comment.ReplyTo);
                if (LastComment == null)
                {
                    throw new Exception("回复对象不存在！");
                }
                else
                {
                    LastCommentStatus = LastComment.Status;
                }
            }
            if (LastCommentStatus == (int)SysStatusEnum.Normal)
            {
                return _scope.Insertable(newComment).ExecuteCommand();
            }
            else if (Enum.TryParse<SysStatusEnum>(LastCommentStatus.ToString(), out SysStatusEnum res))
            {
                throw new Exception($"该评论已 {res.GetRemark()},无法回复！");
            }
            else
            {
                throw new Exception($"该评论状态已不允许回复！{LastCommentStatus}");
            }
        }
        public TblComment? GetComment(int id)
        {
            return _scope.Queryable<TblComment>().InSingle(id);
        }
        public TblSubComment? GetSubComment(int id)
        {
            return _scope.Queryable<TblSubComment>().InSingle(id);
        }
    }
}
