using LibFrame.DTOModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibFrame.IServices
{
    public interface IRegisterService
    {
        public LoginRegisterResultModel RegisterAccount(LoginRegisterModel model);
    }
}
