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

        public async Task<Patient> AddOrUpdate(Patient patient)
        {
            if (await _context.Patients.FirstOrDefaultAsync(d => d.Id.Equals(patient.Id)) is { } obj)
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

            await _context.SaveChangesAsync();

            return patient;
        }

        public async Task Delete(int? id)
        {
            Patient patient = await _context
                .Patients
                .FirstOrDefaultAsync(p => p.Id == id);

            _context.Patients.Remove(patient);
            await _context.SaveChangesAsync();
        }

        public async Task<IQueryable<Patient>> GetAllPatientsWithDiet(PatientQuery query) => await Task.FromResult( _context
        .Patients
        .Include(p => p.Diet)
        .Where(r => query.SearchPhrase == null
                                || r.FirstName.ToLower().Contains(query.SearchPhrase.ToLower())
                                || r.LastName.ToLower().Contains(query.SearchPhrase.ToLower()))); 

        public async Task<Patient> GetPatientWithDiet(int? id) => await _context
            .Patients
            .Include(p => p.Diet)
            .FirstOrDefaultAsync(p => p.Id == id);
    }
}
