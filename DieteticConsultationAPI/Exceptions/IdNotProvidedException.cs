using System.Net;
namespace DieteticConsultationAPI.Exceptions;

public sealed class IdNotProvidedException : CommonHttpException
{
    private IdNotProvidedException() : base("Id not provided", HttpStatusCode.NotFound)
    {

    }

    public static void For() =>
        throw new IdNotProvidedException();
}