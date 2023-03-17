using LibFrame.DBModel;
using LibFrame.DTOModel;
using LibFrame.IServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibFrame.Services
{
    public class LoginByAccountPwd : ILoginService
    {
        private readonly UserService _userService;
        private readonly IGenerateJwtToken _generateJwtToken;
        public LoginByAccountPwd(UserService userService, IGenerateJwtToken generateJwtToken)
        {
            _userService = userService;
            _generateJwtToken = generateJwtToken;
        }
        public LoginRegisterResultModel LoginAccount(LoginRegisterModel model)
        {
            LoginRegisterResultModel resultModel = new LoginRegisterResultModel();
            if (string.IsNullOrEmpty(model.Account))
            {
                resultModel.Res = $"账号不能为空！ {model.Account}";
            }
            else
            {
                TblUser? tblUser = _userService.GetTblUser(u => u.SID == model.Account || u.Phone == model.Account || u.Email== model.Account);
                if(tblUser != null)
                {
                    if (tblUser.Password == model.Password)
                    {
                        resultModel.User = tblUser;
                        resultModel.Uid = tblUser.ID;
                        resultModel.Res = _generateJwtToken.GenerateToken(tblUser);
                        resultModel.Success = true;
                    }
                    else
                    {
                        resultModel.Res = "密码错误！";
                    }
                }
                else
                {
                    resultModel.Res = $"账号{model.Account}不存在！";
                }
            }
            return resultModel;
        }
    }
}
