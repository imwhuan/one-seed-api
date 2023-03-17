using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibFrame.Attributes
{
    [AttributeUsage(AttributeTargets.Field)]
    internal class RemarkAttribute : Attribute
    {
        private readonly string _Rmark;
        public RemarkAttribute(string remark)
        {
            _Rmark = remark;
        }

        public string GetRemark() => _Rmark;
    }
}
