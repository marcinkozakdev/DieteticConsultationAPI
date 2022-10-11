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

        public async Task<User> Login(LoginDto dto) => await _context
            .Users
            .Include(u => u.Role)
            .FirstOrDefaultAsync(x => x.Email == dto.Email);

        public async Task Register(User newUser)
        {
            await _context.Users.AddAsync(newUser);
            await _context.SaveChangesAsync();
        } 
    }
}
