using DieteticConsultationAPI.Entities;
using DieteticConsultationAPI.Models;
using DieteticConsultationAPI.Repositories.Abstractions;
using Microsoft.EntityFrameworkCore;

namespace DieteticConsultationAPI.Repositories
{
    internal sealed class AccountRepository : IAccountRepository
    {
        private readonly DieteticConsultationDbContext _context;

        public AccountRepository(DieteticConsultationDbContext context)
        {
            _context = context;
        }

        public User? Login(LoginDto dto) => _context
            .Users
            .Include(u => u.Role)
            .FirstOrDefault(x => x.Email == dto.Email);

        public void Register(User newUser)
        {
            _context.Users.Add(newUser);
            _context.SaveChanges();
        } 
    }
}
