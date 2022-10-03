namespace DieteticConsultationAPI
{
    public class AuthenticationSettings
    {
        public string JwtKey { get; set; } = null!;
        public int JwtExpireDays { get; set; } = 1;
        public string JwtIssuer { get; set; } = null;  
    }
}
