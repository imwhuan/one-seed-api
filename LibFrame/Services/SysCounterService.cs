using LibFrame.DBModel;
using LibFrame.Exceptions;
using SqlSugar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibFrame.Services
{
    public class SysCounterService
    {
        private readonly SqlSugarScope _scope;
        public SysCounterService(SqlSugarScope scope)
        {
            _scope = scope;
        }
        internal string GetCounter(string Id)
        {
            _scope.BeginTran();
            SysCounter counter= _scope.Queryable<SysCounter>().TranLock(DbLockType.Wait).InSingle(Id);
            if (counter == null)
            {
                _scope.CommitTran();
                throw new SysFrameDataException($"系统不存在Counter:{Id}");
            }
            else
            {
                if (counter.MaxVal > counter.MinVal)
                {
                    counter.MinVal = ++counter.MinVal;
                    _scope.Updateable<SysCounter>(counter).ExecuteCommand();
                    _scope.CommitTran();
                    return $"{counter.PreStr}{counter.MinVal}{counter.EndStr}";
                }
                else
                {
                    throw new SysFrameDataException($"Counter:{Id}队列已达到最大值！");
                }
            }
            
        }

        internal string GenerateNewSID()
        {
            string SID = GetCounter("SID");
            bool exit = _scope.Queryable<TblUser>().Where(u => u.SID == SID).Count() > 0;
            if (exit)
            {
                throw new SysFrameDataException($"SID分发服务出现异常！重复分配SID：{SID}");
                //return GenerateNewSID();
            }
            else
            {
                return SID;
            }
        }
    }
}
