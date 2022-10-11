using System.Net;
namespace DieteticConsultationAPI.Exceptions;

internal sealed class CannotFindResourceException : CommonHttpException
{
    private CannotFindResourceException(int id) : base($"Cannot find resource for id:{id}", HttpStatusCode.NotFound)
    {

    }

    public static void For(int id) =>
        throw new CannotFindResourceException(id);
}