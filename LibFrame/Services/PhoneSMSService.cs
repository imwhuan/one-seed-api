using LibFrame.Common;
using LibFrame.IServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace LibFrame.Services
{
    public class PhoneSMSService : ISMSService
    {
        public string SendMsg(string account, string msg)
        {
            Thread.Sleep(1000);
            return "ok";
        }

        /// <summary>
        /// 验证手机号格式
        /// </summary>
        /// <param name="account"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public (bool, string) ValidateAccount(string account)
        {
            if (RegexHelper.CheckPhone(account) == false)
            {
                return (false, $"手机号{account}格式不合法");
            }
            return (true, account);
        }
    }
}
