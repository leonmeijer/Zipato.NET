using System;
using System.Net;

namespace LVMS.Zipato.Exceptions
{
    public class RequestFailedException : ZipatoException
    {
        public HttpStatusCode StatusCode;

        public RequestFailedException(HttpStatusCode statusCode) : base()
        {
            StatusCode = statusCode;
        }

        public RequestFailedException(string message, Exception innerException) : base(message, innerException)
        {
            
        }
    }
}
