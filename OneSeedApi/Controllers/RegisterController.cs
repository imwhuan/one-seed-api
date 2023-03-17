using Autofac;
using LibFrame.DTOModel;
using LibFrame.Enums;
using LibFrame.IServices;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using OneSeedApi.Filters;

namespace OneSeedApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RegisterController : ControllerBase
    {
        private readonly IComponentContext _componentContext;
        public RegisterController(IComponentContext componentContext)
        {
            _componentContext=componentContext;
        }
        /// <summary>
        /// 注册
        /// </summary>
        /// <param name="model">注册数据</param>
        /// <returns>SID</returns>
        [HttpPost]
        public ActionResult<string> Post(LoginRegisterModel model)
        {
            LoginRegisterResultModel resultModel = GetRegisterService(model.AccountNumberType).RegisterAccount(model);
            if (resultModel.Success)
            {
                return resultModel.Res;
            }
            else
            {
                return new BadRequestObjectResult(resultModel.Res);
            }
        }
        private IRegisterService GetRegisterService(AccountNumberTypeEnum accountNumberType)
        {
            if (_componentContext.IsRegisteredWithKey<IRegisterService>(accountNumberType))
            {
                return _componentContext.ResolveKeyed<IRegisterService>(accountNumberType);
            }
            else
            {
                throw new BadHttpRequestException($"系统不支持的注册方式：{accountNumberType.GetRemark()}");
            }
        }
    }
}
