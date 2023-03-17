using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace LibFrame.Common
{
    internal class RegexHelper
    {
        /// <summary>
        /// 检查字符串是否为手机号格式
        /// </summary>
        /// <param name="str"></param>
        /// <returns>是否</returns>
        public static bool CheckPhone(string str)
        {
            return new Regex("^1[34578][0-9]{9}$").IsMatch(str);
        }

        /// <summary>
        /// 检查字符串是否为邮箱格式
        /// </summary>
        /// <param name="str"></param>
        /// <returns>是否</returns>
        public static bool CheckEmail(string str)
        {
            return new Regex("^[a-zA-Z0-9_-]+@[a-zA-Z0-9_-]+(\\.[a-zA-Z0-9_-]+)+").IsMatch(str);
        }
    }
}
