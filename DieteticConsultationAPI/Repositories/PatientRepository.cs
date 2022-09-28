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
            if (_context.Patients.FirstOrDefault(d => d.Id.Equals(patient.Id)) is { } obj)
            {
                obj.FirstName = patient.FirstName;
                obj.LastName = patient.LastName;
                obj.ContactNumber = patient.ContactNumber;
                obj.ContactEmail = patient.ContactEmail;
                obj.Age = patient.Age;
                obj.Weight = patient.Weight;
                obj.Height = patient.Height;
                obj.Sex = patient.Sex;
                obj.Diet=patient.Diet;

                _context.Update(obj);
            }

            else
                _context.Patients.Add(patient);

            _context.SaveChanges();

            return patient;
        }

        public void Delete(int? id)
        {
            Patient? patient = _context
                .Patients
                .FirstOrDefault(p => p.Id == id);

            _context.Patients.Remove(patient);
            _context.SaveChanges();
        }

        public IQueryable<Patient> GetAllPatientsWithDiet(PatientQuery query) => _context
        .Patients
        .Include(p => p.Diet)
        .Where(r => query.SearchPhrase == null
                                || r.FirstName.ToLower().Contains(query.SearchPhrase.ToLower())
                                || r.LastName.ToLower().Contains(query.SearchPhrase.ToLower()));

        public Patient? GetPatientWithDiet(int? id) => _context
            .Patients
            .Include(p => p.Diet)
            .FirstOrDefault(p => p.Id == id);
    }
}
