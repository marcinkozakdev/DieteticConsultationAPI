using DieteticConsultationAPI.Entities;
using DieteticConsultationAPI.Models.Pagination;
using DieteticConsultationAPI.Repositories.Abstractions;
using Microsoft.EntityFrameworkCore;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace DieteticConsultationAPI.Repositories
{
    public class PatientRepository : IPatientRepository
    {
        private readonly DieteticConsultationDbContext _context;

        public PatientRepository(DieteticConsultationDbContext context)
        {
            _context = context;
        }

        public Patient? AddOrUpdate(Patient patient)
        {
            // same bullshit
            _context.Patients.Add(patient);
            _context.SaveChanges();

            if (patient != null)
                _context.Patients.Update(patient);
            _context.SaveChanges();

            return patient;
        }

        public void Delete(int? id)
        {
            var patient = _context
                .Patients
                .FirstOrDefault(p => p.Id == id);

            _context.Patients.Remove(patient);
            _context.SaveChanges();
        }

        // change name, you are returning something else besides patients
        public IQueryable<Patient> GetAll(PatientQuery query) => _context
        .Patients
        .Include(p => p.Diet)
        .Where(r => query.SearchPhrase == null
                                || r.FirstName.ToLower().Contains(query.SearchPhrase.ToLower())
                                || r.LastName.ToLower().Contains(query.SearchPhrase.ToLower()));

        // change name 
        public Patient? GetById(int? id) => _context
            .Patients
            .Include(p => p.Diet)
            .FirstOrDefault(p => p.Id == id);

    }
}
