namespace DieteticConsultationAPI.Exceptions;

// you should extended all exception with this common one 
public class CommonException : Exception
{
    private CommonException() : base("Something bas happened to our server")
    {
    }
}