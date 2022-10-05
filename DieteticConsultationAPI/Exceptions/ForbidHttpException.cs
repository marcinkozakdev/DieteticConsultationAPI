using System.Net;

namespace DieteticConsultationAPI.Exceptions
{
    internal sealed class ForbidHttpException : CommonHttpException
    {

        private ForbidHttpException(string message) : base(message, HttpStatusCode.Forbidden)
        {
        }

        private ForbidHttpException() : base(HttpStatusCode.Forbidden)
        {
        }

        public static void For(string message) => throw new ForbidHttpException(message);
    }
}