using SqlSugar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibFrame.DBModel
{
    [SugarTable("sys_visitrecord")]
    public class SysVisitRecord
    {
        [SugarColumn(IsPrimaryKey = true, IsIdentity = true)]
        public int ID { get; set; }
        public string IP { get; set; } = "";
        public string? Province { get; set; }
        public string? City { get; set; }
        public int Times { get; set; }
        public string? Remarks { get; set; }
        public DateTime? CreateDate { get; set; }
        public DateTime? ModifyDate { get; set; }
    }
}
