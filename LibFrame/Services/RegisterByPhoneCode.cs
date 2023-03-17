using LibFrame.Common;
using LibFrame.DBModel;
using LibFrame.DTOModel;
using LibFrame.Enums;
using LibFrame.IServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace LibFrame.Services
{
    public class RegisterByPhoneCode: IRegisterService
    {
        private readonly UserService _userService;
        private readonly VerifyCodeService _verifyCodeService;
        private readonly SysCounterService _sysCounterService;
        public RegisterByPhoneCode(UserService userService, VerifyCodeService verifyCodeService, SysCounterService sysCounterService)
        {
            _userService = userService;
            _verifyCodeService = verifyCodeService;
            _sysCounterService = sysCounterService;
        }

        public LoginRegisterResultModel RegisterAccount(LoginRegisterModel model)
        {
            LoginRegisterResultModel resultModel = new LoginRegisterResultModel();
            if (RegexHelper.CheckPhone(model.Account))
            {
                //判断验证码是否正确
                string oldCode= _verifyCodeService.GetVerifyCode(model.Account, AccountActionTypeEnum.Register);
                if(oldCode == model.Code)
                {
                    Expression<Func<TblUser, bool>> expression = u => u.Phone == model.Account;
                    bool exit = _userService.CheckUserExit(expression);
                    if (exit)
                    {
                        resultModel.Res = $"注册失败！{model.AccountNumberType.GetRemark()} {model.Account} 已经存在！";
                    }
                    else
                    {
                        string SID = _sysCounterService.GenerateNewSID();
                        TblUser user = new TblUser()
                        {
                            SID = SID,
                            Phone = model.Account,
                            Password = model.Password,
                            Name = SID,
                        };
                        int uid= _userService.AddUser(user);
                        resultModel.Uid = uid;
                        resultModel.Res = SID;
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
