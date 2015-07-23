using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace LVMS.Zipato.Exceptions
{
    public class RequestFailedException : ZipatoException
    {
        public HttpStatusCode StatusCode;

        public RequestFailedException(HttpStatusCode statusCode) : base()
        {
            StatusCode = statusCode;
        }
    }
}
