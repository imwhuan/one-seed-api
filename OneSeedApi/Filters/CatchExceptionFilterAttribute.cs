using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc;
using OneSeedApi.Model;

namespace OneSeedApi.Filters
{
    /// <summary>
    /// 捕获控制器中的异常使异常结果按标准格式返回
    /// </summary>
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
    public class CatchExceptionFilterAttribute : Attribute, IExceptionFilter
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="context"></param>
        public void OnException(ExceptionContext context)
        {
            context.ExceptionHandled = true;
            ApiResult apiResult = new ApiResult()
            {
                StatusCode = StatusCodes.Status500InternalServerError,
                Message = context.Exception.Message
            };
            if (context.Exception is BadHttpRequestException httpException)
            {
                apiResult.StatusCode = httpException.StatusCode;
            }
            ObjectResult result = new(apiResult)
            {
                StatusCode = apiResult.StatusCode
            };
            context.Result = result;
        }
    }
}
