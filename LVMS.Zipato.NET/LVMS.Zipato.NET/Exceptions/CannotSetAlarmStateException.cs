using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LVMS.Zipato.Exceptions
{
    public class CannotSetAlarmStateException : ZipatoException
    {
        public CannotSetAlarmStateException(string message) : base(message)
        {

        }
    }
}
