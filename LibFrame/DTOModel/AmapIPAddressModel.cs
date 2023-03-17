using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibFrame.DTOModel
{
    public class AmapIPAddressModel
    {
        /// <summary>
        /// 值为0或1,0表示失败；1表示成功
        /// </summary>
        public string? status { get; set; }
        /// <summary>
        /// 返回状态说明，status为0时，info返回错误原因，否则返回“OK”。
        /// </summary>
        public string? info { get; set; }
        /// <summary>
        /// 返回状态说明,10000代表正确,详情参阅info状态表
        /// </summary>
        public string? infocode { get; set; }
        /// <summary>
        /// 省份名称
        /// 若为直辖市则显示直辖市名称；
        /// 如果在局域网 IP网段内，则返回“局域网”；
        /// 非法IP以及国外IP则返回空
        /// </summary>
        public object? province { get; set; }
        /// <summary>
        /// 城市名称
        /// 若为直辖市则显示直辖市名称；
        /// 如果为局域网网段内IP或者非法IP或国外IP，则返回空
        /// </summary>
        public object? city { get; set; }
        /// <summary>
        /// 城市的adcode编码
        /// </summary>
        public object? adcode { get; set; }
        /// <summary>
        /// 所在城市范围的左下右上对标对
        /// </summary>
        public object? rectangle { get; set; }
    }
}
