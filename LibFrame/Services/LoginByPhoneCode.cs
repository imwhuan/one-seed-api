using LibFrame.Common;
using LibFrame.DBModel;
using LibFrame.DTOModel;
using LibFrame.Enums;
using LibFrame.Exceptions;
using LibFrame.IServices;
using SqlSugar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace LibFrame.Services
{
    public class LoginByPhoneCode : ILoginService
    {
        private readonly SqlSugarScope _scope;
        private readonly VerifyCodeService _verifyCodeService;
        private readonly IGenerateJwtToken _generateJwtToken;
        public LoginByPhoneCode(SqlSugarScope scope, VerifyCodeService verifyCodeService, IGenerateJwtToken generateJwtToken)
        {
            _scope = scope;
            _verifyCodeService = verifyCodeService;
            _generateJwtToken = generateJwtToken;
        }
        public LoginRegisterResultModel LoginAccount(LoginRegisterModel model)
        {
            LoginRegisterResultModel resultModel = new LoginRegisterResultModel();
            if (RegexHelper.CheckPhone(model.Account))
            {
                //判断验证码是否正确
                string oldCode = _verifyCodeService.GetVerifyCode(model.Account, AccountActionTypeEnum.Login);
                if (oldCode == model.Code)
                {
                    // Expression<Func<TblUser, bool>> expression = u => u.Phone == model.Account;
                    TblUser tblUser = _scope.Queryable<TblUser>().Where(u => u.Phone == model.Account).First();
                    if (tblUser==null)
                    {
                        resultModel.Res= $"登录失败！{model.AccountNumberType.GetRemark()} {model.Account} 不存在！";
                    }
                    else
                    {
                        resultModel.User=tblUser;
                        resultModel.Uid = tblUser.ID;
                        resultModel.Res = _generateJwtToken.GenerateToken(tblUser);
                        resultModel.Success = true;
                    }
                }
                else
                {
                    resultModel.Res = "验证码错误！";
                }
            }
            else
            {
                resultModel.Res = $"手机号 {model.Account} 为非法格式！";
            }
            return resultModel;
        }
    }
}
