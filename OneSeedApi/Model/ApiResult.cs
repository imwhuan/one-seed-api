using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OneSeedApi.Model
{
    public class ApiResult
    {
        public ApiResult()
        {
            StatusCode = StatusCodes.Status500InternalServerError;
        }

        /// <summary>
        /// 状态码
        /// </summary>
        public int StatusCode { get; set; }
        /// <summary>
        /// 附带消息
        /// </summary>
        public string? Message { get; set; }
        /// <summary>
        /// 返回数据
        /// </summary>
        public object? Data { get; set; }
        /// <summary>
        /// 额外数据
        /// </summary>
        public object? Tag { get; set; }
        /// <summary>
        /// 请求是否成功
        /// </summary>
        public bool Success { get => StatusCode.ToString().StartsWith('2'); }
    }
}
