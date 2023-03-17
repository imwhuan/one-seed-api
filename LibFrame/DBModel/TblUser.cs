using LibFrame.Common;
using SqlSugar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibFrame.DBModel
{
    [SugarTable("u_user")]
    public class TblUser
    {
        [SugarColumn(IsPrimaryKey = true, IsIdentity = true)]
        public int ID { get; set; }

        [SugarColumn(Length = 30)]
        public string? SID { get; set; }

        [SugarColumn(Length = 30)]
        public string? Name { get; set; }

        [SugarColumn(Length = 20)]
        public string? Password { get; set; }

        [SugarColumn(Length = 12)]
        public string? Phone { get; set; }

        [SugarColumn(Length = 30)]
        public string? Email { get; set; }


        [SugarColumn(Length = 50)]
        public string? Tag { get; set; }
        /// <summary>
        /// 用户头像
        /// </summary>
        [SugarColumn(Length = 100)]
        public string? HeadImg { get; set; }

        [SugarColumn(Length = 100)]
        public string? Describe { get; set; }
        [Navigate(NavigateType.OneToOne, nameof(ID))]
        public TblUserInfo? UserInfo { get; set; }
        [SugarColumn(IsIgnore =true)]
        public string[] Tags { get
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
            set { Tag = string.Join(GlobalConstData.UserTagsSplit, value); } }
    }
}
