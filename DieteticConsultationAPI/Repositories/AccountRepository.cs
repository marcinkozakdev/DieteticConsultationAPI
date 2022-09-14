using DieteticConsultationAPI.Entities;
using DieteticConsultationAPI.Models;
using DieteticConsultationAPI.Repositories.Abstractions;
using Microsoft.EntityFrameworkCore;

namespace DieteticConsultationAPI.Repositories
{
    public class AccountRepository : IAccountRepository
    {
        private readonly DieteticConsultationDbContext _context;

        public AccountRepository(DieteticConsultationDbContext context)
        {
            _context = context;
        }

        public void LoginUser(LoginDto dto) =>
           _context.Users.Include(u => u.Role).FirstOrDefault(x => x.Email == dto.Email);
        

        public void RegisterUser()
        {
            throw new NotImplementedException();
        }
    }
}
