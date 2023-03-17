using Microsoft.AspNetCore.Mvc.Filters;

namespace OneSeedApi.Filters
{
    public class LogActionFilterAttribute:ActionFilterAttribute
    {
        public override async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            Console.WriteLine("进入Action");
            await next.Invoke();
            Console.WriteLine("结束Action");
            //return base.OnActionExecutionAsync(context, next);
        }
    }
}
