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
    public class SimulateSMSService : ISMSService
    {
        public string SendMsg(string account, string msg)
        {
            Thread.Sleep(1000);
            return "ok";
        }

        public (bool, string) ValidateAccount(string account)
        {
            if (string.IsNullOrEmpty(account))
            {
                return (false, "账号为空！");
            }
            return (true, account);
        }
    }
}
