using SqlSugar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibFrame.DBModel
{
    [SugarTable("sys_comment")]
    public class TblComment
    {
        [SugarColumn(IsPrimaryKey = true, IsIdentity = true)]
        public int ID { get; set; }
        public int Uid { get; set; }
        public int MediaId { get; set; }
        public int MediaType { get; set; }
        [SugarColumn(Length = 200)]
        public string Content { get; set; } = "";
        public int Status { get; set; }
        public int Like { get; set; }
        public int DisLike { get; set; }
        public DateTime CreateTime { get; set; }

        [Navigate(NavigateType.OneToOne, nameof(Uid))]
        public TblUser? User { get; set; }
    }

    [SugarTable("sys_subcomment")]
    public class TblSubComment
    {
        [SugarColumn(IsPrimaryKey = true, IsIdentity = true)]
        public int ID { get; set; }
        public int Uid { get; set; }
        public int GroupId { get; set; }
        public int ReplyTo { get; set; }
        [SugarColumn(Length = 200)]
        public string Content { get; set; } = "";
        public int Status { get; set; }
        public int Like { get; set; }
        public int DisLike { get; set; }
        public DateTime CreateTime { get; set; }

        [Navigate(NavigateType.OneToOne, nameof(Uid))]
        public TblUser? User { get; set; }
        [Navigate(NavigateType.OneToOne, nameof(ReplyTo))]
        public TblComment? LastComment { get; set; }
    }
}
