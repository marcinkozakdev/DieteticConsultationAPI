using System.Net;
namespace DieteticConsultationAPI.Exceptions;

public sealed class IncorrectLogginException : CommonHttpException
{
    private IncorrectLogginException(int id) : base($"Invalid username or password for id: {id}", HttpStatusCode.BadRequest)
    {
    }

    private IncorrectLogginException() : base(HttpStatusCode.BadRequest)
    {
    }

    public static void For(int id) => throw new IncorrectLogginException(id);
    public static void For() => throw new IncorrectLogginException();

}