using DieteticConsultationAPI.Entities;
using DieteticConsultationAPI.Exceptions;
using DieteticConsultationAPI.Repositories.Abstractions;
using Microsoft.EntityFrameworkCore;

namespace DieteticConsultationAPI.Repositories
{
    public class DieticianRepository : IDieticianRepository
    {
        private readonly DieteticConsultationDbContext _context;
        public DieticianRepository(DieteticConsultationDbContext context) =>
            _context = context;

        public async Task<ICollection<Dietician>> GetAll() =>
            await _context
                .Dieticians
                .Include(d => d.Patients)
                .ToListAsync();

        public async Task<Dietician> GetById(int id) =>
            await _context
                .Dieticians
                .FirstOrDefaultAsync(d => d.Id == id);

        public async Task AddOrUpdate(Dietician dietician)
        {
            if (await _context.Dieticians.FirstOrDefaultAsync(d => d.Id.Equals(dietician.Id)) is { } obj)
            {
                obj.FirstName = dietician.FirstName;
                obj.LastName = dietician.LastName;
                obj.Specialization = dietician.Specialization;
                obj.ContactNumber = dietician.ContactNumber;
                obj.ContactEmail = dietician.ContactEmail;

                _context.Update(obj);
            }

            else
                await _context.Dieticians.AddAsync(dietician);

            await _context.SaveChangesAsync();
        }

        public async Task Delete(int id)
        {
            if (await _context.Dieticians.FirstOrDefaultAsync(d => d.Id == id) is { } dietician)
            {
                _context.Dieticians.Remove(dietician);
                await _context.SaveChangesAsync();
            }
            else
                CannotFindResourceException.For(id);
        }
    }
}