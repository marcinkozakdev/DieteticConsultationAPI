using System.Net;

namespace DieteticConsultationAPI.Exceptions
{
    internal sealed class BadRequestHttpException : CommonHttpException
    {

        private BadRequestHttpException(string message) : base(message, HttpStatusCode.BadRequest)
        {
        }

        private BadRequestHttpException() : base(HttpStatusCode.BadRequest)
        {
        }

        public static void For(string message) => throw new BadRequestHttpException(message);
    }
}