
using DieteticConsultationAPI.Entities;
using DieteticConsultationAPI.Repositories.Abstractions;
using Microsoft.EntityFrameworkCore;

namespace DieteticConsultationAPI.Repositories
{
    public class DieticianRepository : IDieticianRepository
    {
        private readonly DieteticConsultationDbContext _context;

        public DieticianRepository(DieteticConsultationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Dietician>> GetAll() =>
            await _context.Dieticians.Include(d => d.Patients).ToListAsync();

        public async Task<Dietician> GetById(int? id) =>
            await _context.Dieticians.FirstOrDefaultAsync(d => d.Id == id);

        public async Task<Dietician> AddOrUpdate(Dietician dietician)
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
                await _context.AddAsync(dietician);

            await _context.SaveChangesAsync();

            return dietician;
        }

        public async Task Delete(int? id)
        {
            Dietician dietician = await _context.Dieticians.FirstOrDefaultAsync(d => d.Id == id);
            _context.Dieticians.Remove(dietician);
            await _context.SaveChangesAsync();
        }
    }
}
