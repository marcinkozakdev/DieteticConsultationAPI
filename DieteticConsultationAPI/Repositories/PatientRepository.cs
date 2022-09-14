using DieteticConsultationAPI.Entities;
using DieteticConsultationAPI.Repositories.Abstractions;
using Microsoft.EntityFrameworkCore;

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
            _context.Patients.Add(patient);
            _context.SaveChanges();

            if (patient != null)
                _context.Patients.Update(patient);
            _context.SaveChanges();

            return patient;
        }

        public void Delete(int? id)
        {
            Patient? patient = _context.Patients.FirstOrDefault(p => p.Id == id);
            _context.Patients.Remove(patient);
            _context.SaveChanges();
        }

        public ICollection<Patient> GetAll()
        {
            throw new NotImplementedException();
        }

        public ICollection<Patient> GetAllWithDiet()
        {
            throw new NotImplementedException();
        }

        public Patient? GetById(int? id) =>
            _context.Patients.Include(p => p.Diet).FirstOrDefault(p => p.Id == id);

    }
}
