using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibFrame.Exceptions
{
    internal class SysFrameDataException:Exception
    {
        public SysFrameDataException() { }
        public SysFrameDataException(string message) : base(message) { }
        public SysFrameDataException(string message, Exception innerException) : base(message, innerException) { }
    }
}
