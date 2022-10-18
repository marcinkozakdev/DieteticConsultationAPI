using System.Net;
namespace DieteticConsultationAPI.Exceptions;

public sealed class CannotFindResourceException : CommonHttpException
{
    private CannotFindResourceException(int id) : base($"Cannot find resource for id: {id}", HttpStatusCode.NotFound)
    {

    }

    private CannotFindResourceException() : base("Cannot find resource", HttpStatusCode.NotFound)
    {

    }

    public static void For(int id) =>
        throw new CannotFindResourceException(id);

    public static void For() =>
        throw new CannotFindResourceException();
}