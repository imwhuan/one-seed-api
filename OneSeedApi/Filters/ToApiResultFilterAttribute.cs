using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using OneSeedApi.Model;

namespace OneSeedApi.Filters
{
    /// <summary>
    /// 将返回结果转化为ApiResult
    /// </summary>
    [AttributeUsage(AttributeTargets.Method | AttributeTargets.Class, AllowMultiple = false)]
    public class ToApiResultFilterAttribute : Attribute, IResultFilter
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="context"></param>
        public void OnResultExecuted(ResultExecutedContext context)
        {

        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="context"></param>
        public void OnResultExecuting(ResultExecutingContext context)
        {
            if (context.Result is ObjectResult ores)
            {
                if (ores.Value?.GetType() != typeof(ApiResult))
                {
                    ApiResult result = new ApiResult()
                    {
                        StatusCode = ores.StatusCode ?? StatusCodes.Status200OK,
                    };
                    if (result.StatusCode.ToString().StartsWith('2'))
                    {
                        result.Data = ores.Value;
                    }
                    else if (ores.Value is ProblemDetails problem)
                    {
                        result.Message = problem.Title;
                        result.Tag = problem.Detail;
                    }
                    else
                    {
                        result.Data = ores.Value;
                        result.Message = ores.Value?.ToString();
                    }
                    context.Result = new ObjectResult(result) { StatusCode = result.StatusCode };
                }
            }
        }
    }
}
