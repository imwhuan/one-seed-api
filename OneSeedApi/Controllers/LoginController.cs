using Autofac;
using AutoMapper;
using LibFrame.DBModel;
using LibFrame.DTOModel;
using LibFrame.Enums;
using LibFrame.IServices;
using LibFrame.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using OneSeedApi.Model;

namespace OneSeedApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LoginController : ControllerBase
    {
        private readonly IComponentContext _componentContext;
        private readonly LoginRecordManager _loginRecordManager;
        private readonly IMapper _iMapper;
        public LoginController(IComponentContext componentContext, LoginRecordManager loginRecordManager, IMapper iMapper)
        {
            _componentContext = componentContext;
            _loginRecordManager = loginRecordManager;
            _iMapper = iMapper;
        }
        /// <summary>
        /// 登录
        /// </summary>
        /// <param name="model"></param>
        /// <returns>成功：Token，失败：错误信息</returns>
        [HttpPost]
        public ActionResult<LoginResult> Post(LoginRegisterModel model)
        {
            LoginRegisterResultModel resultModel= GetLoginService(model.AccountNumberType).LoginAccount(model);
            if (resultModel.Success && resultModel.User !=null)
            {
                LoginResult result = new LoginResult()
                {
                    Token = resultModel.Res,
                    User = _iMapper.Map<TblUser, UserAccountInfo>(resultModel.User)
                };
                _loginRecordManager.AddLoginRecord(resultModel.Uid, HttpContext.Request.Host.Value);
                return result;
            }
            else
            {
                return new BadRequestObjectResult(resultModel.Res);
            }
        }

        private ILoginService GetLoginService(AccountNumberTypeEnum accountNumberType)
        {
            if (_componentContext.IsRegisteredWithKey<ILoginService>(accountNumberType))
            {
                return _componentContext.ResolveKeyed<ILoginService>(accountNumberType);
            }
            else
            {
                throw new BadHttpRequestException($"系统不支持的登录方式：{accountNumberType.GetRemark()}");
            }
        }
    }
}
