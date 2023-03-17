using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibFrame.DTOModel
{
    public class UserAccountInfo
    {
        public int ID { get; set; }

        public string? SID { get; set; }

        public string? Name { get; set; }

        public string? Phone { get; set; }
        public string? Email { get; set; }
        /// <summary>
        /// 用户头像
        /// </summary>
        public string? HeadImg { get; set; }
        public string? Describe { get; set; }
        public string[] Tags { get; set; } = Array.Empty<string>();
    }
}
