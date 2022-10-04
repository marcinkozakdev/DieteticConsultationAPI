using System.Net;

namespace DieteticConsultationAPI.Exceptions
{
    internal sealed class NotFoundHttpException : CommonHttpException
    {

        private NotFoundHttpException(string message) : base(message, HttpStatusCode.NotFound)
        {
        }

        private NotFoundHttpException() : base(HttpStatusCode.NotFound)
        {
        }

        public static void For(string message) => throw new NotFoundHttpException(message);
    }
}