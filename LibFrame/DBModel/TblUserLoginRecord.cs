using SqlSugar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibFrame.DBModel
{
    [SugarTable("u_loginrecord")]
    internal class TblUserLoginRecord
    {
        public int Uid { get; set; }
        [SugarColumn(Length = 20)]
        public string? LoginAddress { get; set; }
        public DateTime LoginTime { get; set; }
    }
}
