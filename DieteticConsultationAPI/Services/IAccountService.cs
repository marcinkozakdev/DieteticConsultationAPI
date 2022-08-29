using DieteticConsultationAPI.Models;

namespace DieteticConsultationAPI.Services
{
    public interface IAccountService
    {
        void RegisterUser(RegisterUserDto dto);
        string GenerateJwt(LoginDto dto);

    }
}
