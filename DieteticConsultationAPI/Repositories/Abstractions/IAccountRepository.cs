using DieteticConsultationAPI.Entities;
using DieteticConsultationAPI.Models;

namespace DieteticConsultationAPI.Repositories.Abstractions
{
    public interface IAccountRepository
    {
        Task Register(User newUser);
        Task <User> Login(LoginDto dto);
    }
}
