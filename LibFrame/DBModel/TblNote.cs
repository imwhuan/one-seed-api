using LibFrame.Common;
using SqlSugar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibFrame.DBModel
{
    [SugarTable("n_note")]
    public class TblNote
    {
        [SugarColumn(IsPrimaryKey = true, IsIdentity = true)]
        public int ID { get; set; }
        public int UID { get; set; }
        [SugarColumn(Length = 30)]
        public string? Title { get; set; }
        [SugarColumn(Length = 1000)]
        public string? Content { get; set; }
        [SugarColumn(Length = 50)]
        public string? Tag { get; set; }
        public DateTime CreateTime { get; set; }
        [SugarColumn(IsIgnore = true)]
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
    }
}
