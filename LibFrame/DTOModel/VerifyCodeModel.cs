using LibFrame.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibFrame.DTOModel
{
    public class VerifyCodeModel
    {
        ///// <summary>
        ///// 是否发送成功
        ///// </summary>
        //public bool Success { get; set; }=false;
        ///// <summary>
        ///// 成功时为验证码/失败时为错误信息
        ///// </summary>
        //public string? Result { get; set; }
        /// <summary>
        /// 账号
        /// </summary>
        public string? Account { get; set; }
        /// <summary>
        /// 账号类型
        /// </summary>
        public AccountNumberTypeEnum AccountNumberType { get; set; }
        /// <summary>
        /// 动作类型
        /// </summary>
        public AccountActionTypeEnum ActionTypeEnum { get; set; }
    }
}
