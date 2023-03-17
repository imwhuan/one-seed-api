using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using LibFrame.DTOModel;
using LibFrame.Services;
using System.Net;
using Newtonsoft.Json;

namespace OneSeedApi.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class CommonController : ControllerBase
    {
        private readonly VerifyCodeService _verifyCodeService;
        private readonly CommonService _commonService;
        public CommonController(VerifyCodeService verifyCodeService,CommonService commonService)
        {
            _verifyCodeService=verifyCodeService;
            _commonService=commonService;
        }
        /// <summary>
        /// 发送验证码
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult SendVerifyCode(VerifyCodeModel model)
        {
            (bool res,string msg) = _verifyCodeService.SendVertifyCode(model);
            if (res)
            {
                return new OkObjectResult(msg);
            }
            else
            {
                return new BadRequestObjectResult(msg);
            }
        }

        /// <summary>
        /// 访问系统
        /// </summary>
        [HttpGet]
        public async void Visit()
        {
            //if (_contextAccessor.HttpContext != null)
            //{
            //    var context = _contextAccessor.HttpContext.Connection;
            //    if(context.RemoteIpAddress != null)
            //    {
            //        IPAddress address=context.RemoteIpAddress.MapToIPv4();
            //        _commonService.AddVisitRecord(address.ToString());
            //    }
            //}
            Microsoft.Extensions.Primitives.StringValues IpObj;
            if (HttpContext.Request.Headers.TryGetValue("X-Real-IP", out IpObj))
            {
                IPAddress address = IPAddress.Parse(IpObj.First()).MapToIPv4();
                await _commonService.AddVisitRecordAsync(address.ToString());
            }
            else if(HttpContext.Request.Headers.TryGetValue("X-Forwarded-For", out IpObj))
            {
                IPAddress address = IPAddress.Parse(IpObj.First()).MapToIPv4();
                await _commonService.AddVisitRecordAsync(address.ToString());
            }
            else
            {
                string header = JsonConvert.SerializeObject(HttpContext.Request.Headers.ToArray());
                await MyLogService.AddAsync(header);
            }
        }
    }
}
