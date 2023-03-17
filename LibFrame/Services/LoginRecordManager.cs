using LibFrame.DBModel;
using SqlSugar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibFrame.Services
{
    public class LoginRecordManager
    {
        private readonly SqlSugarScope _scope;
        public LoginRecordManager(SqlSugarScope scope)
        {
            _scope = scope;
        }

        public void AddLoginRecord(int uid, string ip)
        {
            TblUserLoginRecord loginRecord = new TblUserLoginRecord()
            {
                Uid = uid,
                LoginAddress = ip,
                LoginTime = DateTime.Now
            };
            _scope.Insertable(loginRecord).ExecuteCommand();
        }
    }
}
