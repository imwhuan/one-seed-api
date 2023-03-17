using LibFrame.IServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using LibFrame.Common;

namespace LibFrame.Services
{
    public class EmailSMSService : ISMSService
    {
        public string SendMsg(string account, string msg)
        {
            Thread.Sleep(2000);
            return "ok";
        }

        /// <summary>
        /// 验证邮箱格式
        /// </summary>
        /// <param name="account"></param>
        /// <returns></returns>
        public (bool, string) ValidateAccount(string account)
        {
            if (!RegexHelper.CheckEmail(account))
            {
                return (false, $"手机号{account}格式不合法");
            }
            return (true, account);
        }
    }
}
