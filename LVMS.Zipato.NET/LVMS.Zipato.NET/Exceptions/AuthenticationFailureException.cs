using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LVMS.Zipato.Exceptions
{
    public class AuthenticationFailureException : ZipatoException
    {
        public AuthenticationFailureException(string message) : base(message)
        {

        }
    }
}
