using LibFrame.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibFrame.Enums
{
    public class CustomEnums
    {
    }
    /// <summary>
    /// 动作类型
    /// </summary>
    public enum AccountActionTypeEnum
    {
        /// <summary>
        /// 手机登录
        /// </summary>
        [Remark("登录")]
        Login = 1,
        /// <summary>
        /// 注册
        /// </summary>
        [Remark("注册")]
        Register,
        /// <summary>
        /// 重置密码
        /// </summary>
        [Remark("重置密码")]
        ResetPwd,
    }
    /// <summary>
    /// 账号类型
    /// </summary>
    public enum AccountNumberTypeEnum
    {
        /// <summary>
        /// SID
        /// </summary>
        [Remark("SID")]
        SID,
        /// <summary>
        /// 手机
        /// </summary>
        [Remark("手机")]
        Phone,
        /// <summary>
        /// 邮箱
        /// </summary>
        [Remark("邮箱")]
        Email,
        /// <summary>
        /// 访客
        /// </summary>
        [Remark("访客")]
        Visitor,
    }

    public enum SysStatusEnum
    {
        /// <summary>
        /// 正常
        /// </summary>
        [Remark("正常")]
        Normal,
        /// <summary>
        /// 冻结
        /// </summary>
        [Remark("冻结")]
        Frozen,
        /// <summary>
        /// 已注销
        /// </summary>
        [Remark("已注销")]
        WrittenOff,
        /// <summary>
        /// 已删除
        /// </summary>
        [Remark("已删除")]
        Deleted,
    }

    /// <summary>
    /// 账号类型
    /// </summary>
    public enum SysMediaContentTypeEnum
    {
        /// <summary>
        /// 留言板
        /// </summary>
        [Remark("留言板")]
        MessageBoard,
        /// <summary>
        /// 博客
        /// </summary>
        [Remark("博客")]
        Blog,
        /// <summary>
        /// 笔记
        /// </summary>
        [Remark("笔记")]
        Note,
        /// <summary>
        /// 社交动态
        /// </summary>
        [Remark("社交动态")]
        Post,
    }
}
