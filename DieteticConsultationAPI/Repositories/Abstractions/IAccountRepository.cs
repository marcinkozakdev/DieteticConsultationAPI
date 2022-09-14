using DieteticConsultationAPI.Models;

namespace DieteticConsultationAPI.Repositories.Abstractions
{
    public interface IAccountRepository
    {
        void RegisterUser();
        void LoginUser(LoginDto dto);
    }
}
