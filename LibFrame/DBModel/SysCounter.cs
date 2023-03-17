using SqlSugar;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibFrame.DBModel
{
    [SugarTable("sys_counter")]
    public class SysCounter
    {
        [SugarColumn(IsPrimaryKey = true)]
        [Required]
        public string Id { get; set; } = "";
        [SugarColumn(Length = 6)]
        public string? PreStr { get; set; }
        public int MaxVal { get; set; }
        public int MinVal { get; set; }
        [SugarColumn(Length = 6)]
        public string? EndStr { get; set; }
        [SugarColumn(Length = 20)]
        public string? CounterDesc { get; set; }

    }
}
