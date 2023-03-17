using Autofac;
using LibFrame.Enums;
using LibFrame.IServices;
using LibFrame.Services;
using ServiceStack.Redis;
using SqlSugar;

namespace OneSeedApi.Services
{
    /// <summary>
    /// 重写VerifyCodeService服务，使之可以使用依赖注入获取SMS
    /// </summary>
    public class VerifyCodeServiceExt : VerifyCodeService
    {
        private readonly IComponentContext _componentContext;
        public VerifyCodeServiceExt(SqlSugarScope scope,IComponentContext componentContext, GetRedisClient getRedisClient) : base(scope, getRedisClient)
        {
            _componentContext = componentContext;
        }
        public override ISMSService GetSMS(AccountNumberTypeEnum accountNumberType)
        {
            if (_componentContext.IsRegisteredWithKey<ISMSService>(accountNumberType))
            {
                return _componentContext.ResolveKeyed<ISMSService>(accountNumberType);
            }
            else
            {
                throw new Exception($"系统不支持指定的SMS：{accountNumberType}");
            }
        }
    }
}
