using LibFrame.Common;
using LibFrame.DBModel;
using LibFrame.DTOModel;
using LibFrame.Enums;
using LibFrame.Exceptions;
using SqlSugar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace LibFrame.Services
{
    public class UserService
    {
        private readonly SqlSugarScope _scope;
        public UserService(SqlSugarScope scope)
        {
            _scope = scope;
        }
        public int AddUser(TblUser tblUser)
        {
            int uid= _scope.Insertable(tblUser).ExecuteReturnIdentity();
            _scope.Insertable<TblUserInfo>(new TblUserInfo { Uid=uid }).ExecuteCommand();
            return uid;
        }
        public TblUser? GetTblUser(Expression<Func<TblUser, bool>> expression)
        {
            return _scope.Queryable<TblUser>().Where(expression).First();
        }
        public TblUser? GetTblUser(int uid)
        {
            return _scope.Queryable<TblUser>().Includes(u=>u.UserInfo).InSingle(uid);
        }
        public bool CheckUserExit(Expression<Func<TblUser, bool>> expression)
        {
            return _scope.Queryable<TblUser>().Where(expression).Count() > 0;
        }
        public int ModifyUser(TblUser user)
        {
            return _scope.Updateable<TblUser>(user).ExecuteCommand();
        }
        public int UpdateUser(TblUser user, Expression<Func<TblUser,object>> expression)
        {
            return _scope.Updateable<TblUser>(user).UpdateColumns(expression).ExecuteCommand();
        }
        public int UpdatePhone(int uid,string phone)
        {
            if (RegexHelper.CheckPhone(phone))
            {
                if (_scope.Queryable<TblUser>().Where(u => u.Phone == phone).Count() > 0)
                {
                    throw new Exception($"手机号{phone}已被使用，无法绑定！");
                }
                TblUser? user = GetTblUser(uid);
                if (user == null)
                {
                    throw new Exception($"当前用户不存在！{uid}");
                }
                user.Phone = phone;
                return _scope.Updateable<TblUser>(user).UpdateColumns(u => new { u.Phone }).ExecuteCommand();
            }
            else
            {
                throw new Exception($"手机号 {phone} 格式不合法！");
            }
        }
        public int UpdateEmail(int uid, string email)
        {
            if (RegexHelper.CheckEmail(email))
            {
                if (_scope.Queryable<TblUser>().Where(u => u.Email == email).Count() > 0)
                {
                    throw new Exception($"邮箱{email}已被使用，无法绑定！");
                }
                TblUser? user = GetTblUser(uid);
                if (user == null)
                {
                    throw new Exception($"当前用户不存在！{uid}");
                }
                user.Email = email;
                return _scope.Updateable<TblUser>(user).UpdateColumns(u => new { u.Email }).ExecuteCommand();
            }
            else
            {
                throw new Exception($"邮箱 {email} 格式不合法！");
            }
        }
    }
}
