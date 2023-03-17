using LibFrame.DBModel;
using LibFrame.DTOModel;
using SqlSugar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibFrame.Services
{
    public class CommonService
    {
        private readonly SqlSugarScope _scope;
        private readonly HttpClientHelper _httpClientHelper;
        public CommonService(SqlSugarScope scope, HttpClientHelper httpClientHelper)
        {
            _scope = scope;
            _httpClientHelper = httpClientHelper;
        }
        /// <summary>
        /// 添加系统访问记录
        /// </summary>
        /// <param name="ip"></param>
        public async Task AddVisitRecordAsync(string ip)
        {
            DateTime dateTime = DateTime.Now;
            SysVisitRecord record = _scope.Queryable<SysVisitRecord>()
                .Where(s => s.IP == ip && s.ModifyDate > dateTime.AddMinutes(-30)).First();
            if(record == null)
            {
                record = new SysVisitRecord()
                {
                    IP = ip,
                    CreateDate = dateTime,
                    ModifyDate = dateTime,
                    Times = 1,
                };
                AmapIPAddressModel amapIPAddress;
                try
                {
                    amapIPAddress = await _httpClientHelper.GetAmapIpAddress(ip);
                }
                catch (Exception ex)
                {
                    amapIPAddress = new AmapIPAddressModel()
                    {
                        status = "0",
                        info = ex.Message,
                    };
                }
                record.Remarks = amapIPAddress.info;
                if (amapIPAddress.status == "1")
                {
                    if(amapIPAddress.province?.GetType() == typeof(string))
                    {
                        record.Province = amapIPAddress.province?.ToString();
                    }
                    if (amapIPAddress.city?.GetType() == typeof(string))
                    {
                        record.City = amapIPAddress.city?.ToString();
                    }
                }
                _scope.Insertable(record).ExecuteCommand();
            }
            else
            {
                record.Times += 1;
                record.ModifyDate = dateTime;
                _scope.Updateable(record).ExecuteCommand();
            }
        }
    }
}
