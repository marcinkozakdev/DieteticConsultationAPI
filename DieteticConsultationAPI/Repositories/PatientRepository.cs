using DieteticConsultationAPI.Entities;
using DieteticConsultationAPI.Exceptions;
using DieteticConsultationAPI.Models.Pagination;
using DieteticConsultationAPI.Repositories.Abstractions;
using Microsoft.EntityFrameworkCore;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace DieteticConsultationAPI.Repositories
{
    public class PatientRepository : IPatientRepository
    {
        private readonly DieteticConsultationDbContext _context;

        public PatientRepository(DieteticConsultationDbContext context) => _context = context;


        public async Task AddOrUpdate(Patient patient)
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
                obj.Diet = patient.Diet;
                obj.DieticianId = patient.DieticianId;

                _context.Update(obj);
            }

            else if (patient.Id is 0)
                await _context.Patients.AddAsync(patient);

            else
                CannotFindResourceException.For(patient.Id);

            await _context.SaveChangesAsync();
        }

        public async Task Delete(int id)
        {
            if (await _context.Patients.FirstOrDefaultAsync(d => d.Id == id) is { } patients)
            {
                _context.Patients.Remove(patients);
                await _context.SaveChangesAsync();
            }
            else
                CannotFindResourceException.For(id);
        }

        public async Task<IQueryable<Patient>> GetAll(PatientQuery query) =>
            await Task.FromResult(_context
                .Patients
                .Include(p => p.Diet)
                .Where(r => query.SearchPhrase == null
                                || r.FirstName.ToLower().Contains(query.SearchPhrase.ToLower())
                                || r.LastName.ToLower().Contains(query.SearchPhrase.ToLower())));

        public async Task<Patient> GetById(int id) =>
            await _context
                .Patients
                .Include(p => p.Diet)
                .FirstOrDefaultAsync(p => p.Id == id);
    }
}
