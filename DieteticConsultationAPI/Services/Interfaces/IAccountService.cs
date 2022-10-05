using DieteticConsultationAPI.Models;

namespace DieteticConsultationAPI.Services.Interfaces
{
    public interface IAccountService
    {
        Task RegisterUser(RegisterUserDto dto);
        Task<string> GenerateJwt(LoginDto dto);
    }
}
