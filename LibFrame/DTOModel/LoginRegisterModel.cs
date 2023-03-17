using LibFrame.DBModel;
using LibFrame.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibFrame.DTOModel
{
    /// <summary>
    /// 登录数据模型
    /// </summary>
    public class LoginRegisterModel
    {
        /// <summary>
        /// 账号
        /// </summary>
        public string Account { get; set; } = "";
        /// <summary>
        /// 账号类型
        /// </summary>
        public AccountNumberTypeEnum AccountNumberType { get; set; }
        /// <summary>
        /// 密码
        /// </summary>
        public string? Password { get; set; }
        /// <summary>
        /// 验证码
        /// </summary>
        public string? Code { get; set; }

    }
    /// <summary>
    /// 登录结果数据模型
    /// </summary>
    public class LoginRegisterResultModel
    {
        public int Uid { get; set; }
        public TblUser? User { get; set; }
        public bool Success { get; set; } = false;
        /// <summary>
        /// 注册结果信息，成功则返回Token，失败则为错误信息
        /// </summary>
        public string Res { get; set; } = "";

    }
}
