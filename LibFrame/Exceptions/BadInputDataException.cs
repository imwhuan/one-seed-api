using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibFrame.Exceptions
{
    internal class BadInputDataException : Exception
    {
        public BadInputDataException() { }
        public BadInputDataException(string message) : base(message) { }
        public BadInputDataException(string message, Exception innerException) : base(message, innerException) { }
    }
}
