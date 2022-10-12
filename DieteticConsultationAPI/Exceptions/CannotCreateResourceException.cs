using System.Net;
namespace DieteticConsultationAPI.Exceptions;

public sealed class CannotCreateResourceException : CommonHttpException
{
    private CannotCreateResourceException(string message) : base($"Cannot create resource for {message}", HttpStatusCode.BadRequest)
    {

    }

    public static CannotCreateResourceException For(string message) => 
        throw new CannotCreateResourceException(message);
}