using System.Security.Claims;

namespace OneSeedApi.Common
{
    public static class CommonExtFunc
    {
        internal static string GetUidByToken(this HttpContext context)
        {
            string? name= context.User.Identity?.Name;
            string? id = context.User.Claims.FirstOrDefault(u => u.Type.Equals(ClaimTypes.NameIdentifier))?.Value;
            if (string.IsNullOrEmpty(id)) throw new BadHttpRequestException("用户身份验证失败！");
            return id;
        }
    }
}
