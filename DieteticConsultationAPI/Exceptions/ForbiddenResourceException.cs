using System.Net;
namespace DieteticConsultationAPI.Exceptions;

public sealed class ForbiddenResourceException : CommonHttpException
{
    private ForbiddenResourceException(string message) : base(message, HttpStatusCode.Forbidden)
    {
    }

    private ForbiddenResourceException() : base(HttpStatusCode.Forbidden)
    {
    }

    public static void For(string message) => throw new ForbiddenResourceException(message);
}