
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

        public IEnumerable<Dietician> GetAll() =>
            _context.Dieticians.Include(d => d.Patients);

        public Dietician? GetById(int? id) =>
            _context.Dieticians.FirstOrDefault(d => d.Id == id);

        public Dietician? AddOrUpdate(Dietician dietician)
        {
            if (_context.Dieticians.FirstOrDefault(d => d.Id.Equals(dietician.Id)) is { } obj)
            {
                obj.FirstName = dietician.FirstName;
                obj.LastName = dietician.LastName;
                obj.Specialization = dietician.Specialization;
                obj.ContactNumber = dietician.ContactNumber;
                obj.ContactEmail = dietician.ContactEmail;

                _context.Update(obj);
            }

            else
                _context.Add(dietician);

            _context.SaveChanges();

            return dietician;
        }

        public void Delete(int? id)
        {
            Dietician? dietician = _context.Dieticians.FirstOrDefault(d => d.Id == id);
            _context.Dieticians.Remove(dietician);
            _context.SaveChanges();
        }
    }
}
