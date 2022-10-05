using System.Net;

namespace DieteticConsultationAPI.Exceptions
{
    internal class CommonHttpException : Exception
    {
        public HttpStatusCode Code { get; }

        public CommonHttpException() : base("Something bad happened to our server")
        {
        }
        public CommonHttpException(string message, HttpStatusCode httpStatusCode) : base(message)
        {
            Code = httpStatusCode;
        }

        public CommonHttpException(Exception ex) : base(ex.Message, ex)
        {
        }

        public CommonHttpException(HttpStatusCode httpStatusCode) : base("default message")
        {
            Code = httpStatusCode;
        }
    }
}
