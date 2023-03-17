using LibFrame.DBModel;
using LibFrame.DTOModel;
using LibFrame.Enums;
using LibFrame.IServices;
using SqlSugar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibFrame.Services
{
    public class LoginByVisitor : ILoginService
    {
        private readonly UserService _userService;
        private readonly VerifyCodeService _verifyCodeService;
        private readonly IGenerateJwtToken _generateJwtToken;
        public LoginByVisitor(UserService userService, VerifyCodeService verifyCodeService, IGenerateJwtToken generateJwtToken)
        {
            _userService = userService;
            _verifyCodeService = verifyCodeService;
            _generateJwtToken = generateJwtToken;
        }
        public LoginRegisterResultModel LoginAccount(LoginRegisterModel model)
        {
            LoginRegisterResultModel resultModel = new LoginRegisterResultModel();
            if (string.IsNullOrEmpty(model.Account))
            {
                resultModel.Res = $"访客信息错误！ {model.Account}";
            }
            else
            {
                //判断验证码是否正确
                string oldCode = _verifyCodeService.GetVerifyCode(model.Account, AccountActionTypeEnum.Login);
                if (oldCode == model.Code)
                {
                    //查询访客是否已经存在
                    TblUser? tblUser = _userService.GetTblUser(u=>u.SID==model.Account);
                    if (tblUser == null)
                    {
                        //访客不存在，则注册访客信息
                        tblUser = new TblUser()
                        {
                            SID = model.Account,
                            Name = model.Account,
                        };
                        int uid = _userService.AddUser(tblUser);
                        tblUser.ID = uid;
                    }
                    resultModel.User = tblUser;
                    resultModel.Uid = tblUser.ID;
                    resultModel.Res = _generateJwtToken.GenerateToken(tblUser);
                    resultModel.Success = true;
                }
                else
                {
                    resultModel.Res = "验证码错误！";
                }
            }
            return resultModel;
        }
    }
}
