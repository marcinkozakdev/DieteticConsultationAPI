using System.Net;
namespace DieteticConsultationAPI.Exceptions;

public sealed class ForbiddenResourceException : CommonHttpException
{
    private ForbiddenResourceException(int id) : base($"Authorization failed for id: {id}", HttpStatusCode.Forbidden)
    {
    }

    private ForbiddenResourceException() : base(HttpStatusCode.Forbidden)
    {
    }

    public static void For(int id) => throw new ForbiddenResourceException(id);
}