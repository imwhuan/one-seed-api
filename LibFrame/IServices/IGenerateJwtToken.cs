using LibFrame.DBModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibFrame.IServices
{
    public interface IGenerateJwtToken
    {
        public string GenerateToken(TblUser tblUser);
    }
}
