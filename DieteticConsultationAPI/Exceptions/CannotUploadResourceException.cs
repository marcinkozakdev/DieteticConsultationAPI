using System.Net;
namespace DieteticConsultationAPI.Exceptions;

public sealed class CannotUploadResourceException : CommonHttpException
{
    private CannotUploadResourceException(string message) : base($"Cannot upload file: {message}", HttpStatusCode.BadRequest)
    {

    }

    public static void For(string message) =>
        throw new CannotUploadResourceException(message);
}