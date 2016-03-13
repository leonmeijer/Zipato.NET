using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LVMS.Zipato.Exceptions
{
    public class ZipatoException : Exception
    {
        public ZipatoException()
        {

        }

        public ZipatoException(string message ) : base(message)
        {

        }

        public ZipatoException(string message, Exception innerException) : base(message, innerException)
        {

        }
    }
}
