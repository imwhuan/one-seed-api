using LibFrame.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace LibFrame.Enums
{
    public static class EnumHelperExt
    {
        public static string GetRemark(this Enum value)
        {
            FieldInfo? field = value.GetType().GetField(value.ToString());
            if (field != null)
            {
                RemarkAttribute? remarkAttribute = field.GetCustomAttribute<RemarkAttribute>();
                return remarkAttribute?.GetRemark() ?? value.ToString();
                //if(remarkAttribute == null)
                //{
                //    return value.ToString();
                //}
                //else
                //{
                //    return remarkAttribute.GetRemark();
                //}
                //return remarkAttribute?.GetRemark() ?? value.ToString();
                //if (remarkAttribute != null)
                //{
                //    return remarkAttribute?.GetRemark();
                //}
            }
            return string.Empty;
        }
    }
}
