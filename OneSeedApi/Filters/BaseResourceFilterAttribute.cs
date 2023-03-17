using Microsoft.AspNetCore.Mvc.Filters;

namespace OneSeedApi.Filters
{
    public class BaseResourceFilterAttribute : Attribute, IResourceFilter
    {
        void IResourceFilter.OnResourceExecuted(ResourceExecutedContext context)
        {
            throw new NotImplementedException();
        }

        void IResourceFilter.OnResourceExecuting(ResourceExecutingContext context)
        {
            throw new NotImplementedException();
        }
    }
}
