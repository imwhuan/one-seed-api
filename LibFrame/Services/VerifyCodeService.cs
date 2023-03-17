using LibFrame.DBModel;
using LibFrame.DTOModel;
using LibFrame.Enums;
using LibFrame.IServices;
using ServiceStack.Redis;
using SqlSugar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace LibFrame.Services
{
    public class VerifyCodeService
    {
        //private ISMSService? _SMSService;
        private readonly SqlSugarScope _scope;
        //private readonly IRedisClient _redisClient;
        GetRedisClient _getRedisClient;
        public VerifyCodeService(SqlSugarScope scope, GetRedisClient getRedisClient)
        {
            _scope = scope;
            _getRedisClient=getRedisClient;
            //_redisClient = getRedisClient.GetClient();
        }
        /// <summary>
        /// 发送验证码
        /// </summary>
        /// <param name="model"></param>
        /// <returns>成功则返回验证码，失败返回错误信息</returns>
        public (bool, string) SendVertifyCode(VerifyCodeModel model)
        {
            if (string.IsNullOrEmpty(model.Account))
            {
                return (false, "账号不能为空！");
            };
            //查看验证码是否已经发送，若已发送则不允许重复发送
            string oldCode=GetVerifyCode(model.Account,model.ActionTypeEnum);
            if (!string.IsNullOrEmpty(oldCode)) return (false, "验证码已发送，请勿重复获取");
            ISMSService SMS = GetSMS(model.AccountNumberType);
            //验证账号格式
            (bool checkres, string checkmsg) = SMS.ValidateAccount(model.Account);
            if (!checkres) return (false, checkmsg);
            //验证账号是否存在
            (checkres, checkmsg) = ValidateAccount(model.Account, model.AccountNumberType, model.ActionTypeEnum);
            if (!checkres) return (false, checkmsg);
            //发送验证码
            string code = GenerateRandomCode();
            SMS.SendMsg(model.Account, code);
            //将验证码保存到Redis中
            SaveVerifyCode(model.Account, model.ActionTypeEnum, code);
            return (true, code);
        }

        /// <summary>
        /// 生成随机验证码
        /// </summary>
        /// <returns></returns>
        private static string GenerateRandomCode()
        {
            Random r = new Random();
            return r.Next(10000, 99999).ToString();
        }

        /// <summary>
        /// 根据不同的账号类型，获取不同的SMS服务
        /// </summary>
        /// <param name="codeTypeEnum"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public virtual ISMSService GetSMS(AccountNumberTypeEnum accountNumberType)
        {
            return accountNumberType switch
            {
                AccountNumberTypeEnum.SID => new SimulateSMSService(),
                AccountNumberTypeEnum.Phone => new SimulateSMSService(),
                AccountNumberTypeEnum.Email => new EmailSMSService(),
                AccountNumberTypeEnum.Visitor => new SimulateSMSService(),
                _ => throw new Exception($"不支持的账号类型！{accountNumberType}")
            };
        }

        private (bool, string) ValidateAccount(string account, AccountNumberTypeEnum accountNumberType, AccountActionTypeEnum actionTypeEnum)
        {
            if(accountNumberType == AccountNumberTypeEnum.Visitor)
            {
                return (true, account);
            }
            Expression<Func<TblUser, bool>> expression = accountNumberType switch
            {
                AccountNumberTypeEnum.SID => u => u.ID == int.Parse(account),
                AccountNumberTypeEnum.Phone => u => u.Phone == account,
                AccountNumberTypeEnum.Email => u => u.Email == account,
                _ => throw new Exception($"不支持的账号类型：{accountNumberType}"),
            };
            bool exit = _scope.Queryable<TblUser>().Where(expression).Count() > 0;
            return actionTypeEnum switch
            {
                AccountActionTypeEnum.Login => exit ? (true, "") : (false, $"账号{account}不存在"),
                AccountActionTypeEnum.Register => exit ? (false, $"账号{account}已存在") : (true, ""),
                AccountActionTypeEnum.ResetPwd => exit ? (true, "") : (false, $"账号{account}不存在"),
                _ => throw new Exception($"不支持的操作类型：{actionTypeEnum}"),
            };
        }
        public string GetVerifyCode(string account, AccountActionTypeEnum actionTypeEnum)
        {
            using IRedisClient _redisClient = _getRedisClient.GetClient();
            return _redisClient.Get<string>(GetFullAccountByPre(account, actionTypeEnum));
        }
        private bool SaveVerifyCode(string account, AccountActionTypeEnum actionTypeEnum, string code)
        {
            using IRedisClient _redisClient = _getRedisClient.GetClient();
            return _redisClient.Set<string>(GetFullAccountByPre(account, actionTypeEnum), code, TimeSpan.FromMinutes(3));
        }
        /// <summary>
        /// 获取账号加上前缀后的全称，做redis的key使用
        /// </summary>
        /// <param name="account"></param>
        /// <param name="actionTypeEnum"></param>
        /// <returns></returns>
        private string GetFullAccountByPre(string account, AccountActionTypeEnum actionTypeEnum)
        {
            return $"{actionTypeEnum}_{account}";
        }
    }
}
