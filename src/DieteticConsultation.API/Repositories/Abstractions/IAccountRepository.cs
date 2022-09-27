using DieteticConsultationAPI.Entities;
using DieteticConsultationAPI.Models;

namespace DieteticConsultationAPI.Repositories.Abstractions
{
    public interface IAccountRepository
    {
        void Register(User newUser);
        User? Login(LoginDto dto);
    }
}
