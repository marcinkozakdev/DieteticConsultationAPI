using System.Net;
namespace DieteticConsultationAPI.Exceptions;

public sealed class IncorrectLogginException : CommonHttpException
{
    private IncorrectLogginException(string message) : base(message, HttpStatusCode.BadRequest)
    {
    }

    private IncorrectLogginException() : base(HttpStatusCode.BadRequest)
    {
    }

    public static void For(string message) => throw new IncorrectLogginException(message);
}