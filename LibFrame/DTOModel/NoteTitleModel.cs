using LibFrame.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibFrame.DTOModel
{
    public class NoteTitleModel
    {
        public int Id { get; set; }
        public int UID { get; set; }
        public string? Title { get; set; }
        public string? Tag { get; set; }
        public string[] Tags
        {
            get
            {
                if (string.IsNullOrEmpty(Tag))
                {
                    return Array.Empty<string>();
                }
                else
                {
                    return Tag.Split(GlobalConstData.UserTagsSplit);
                }
            }
            set { Tag = string.Join(GlobalConstData.UserTagsSplit, value); }
        }
        public DateTime CreateTime { get; set; }
    }
}
