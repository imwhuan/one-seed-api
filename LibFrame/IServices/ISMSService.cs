using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibFrame.IServices
{
    public interface ISMSService
    {
        public string SendMsg(string account,string msg);

        /// <summary>
        /// 验证账号格式
        /// </summary>
        /// <param name="account"></param>
        /// <returns></returns>
        public (bool, string) ValidateAccount(string account);
    }
}
