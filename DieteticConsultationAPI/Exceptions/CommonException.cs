namespace DieteticConsultationAPI.Exceptions
{
    public class CommonException : Exception
    {
        private CommonException() : base("Something bad happened to our server")
        {
        }
    }
}
