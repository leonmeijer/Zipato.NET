using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LVMS.Zipato.Exceptions
{
    public class CannotChangeStateException : ZipatoException
    {
        public CannotChangeStateException(string message) : base(message)
        {

        }
    }
}
