using AutoMapper;
using DieteticConsultationAPI.Entities;
using DieteticConsultationAPI.Exceptions;
using DieteticConsultationAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace DieteticConsultationAPI.Services
{
    public class PatientService : IPatientService
    {
        private readonly DieteticConsultationDbContext _context;
        private readonly ILogger<PatientService> _logger;

        public PatientService(DieteticConsultationDbContext context, ILogger<PatientService> logger)
        {
            _context = context;
            _logger = logger;
        }

        public int CreatePatient(CreatePatientDto dto)
        {
            var patient = new Patient
            {
                Id = dto.Id,
                FirstName = dto.FirstName,
                LastName = dto.LastName,
                ContactEmail = dto.ContactEmail,
                ContactNumber = dto.ContactNumber,
                Sex = dto.Sex,
                Age = dto.Age,
                Weight = dto.Weight,
                Height = dto.Height,
                DieticianId = dto.DieticianId,
            };

            _context.Patients.Add(patient);
            _context.SaveChanges();

            return patient.Id;
        }

        public IEnumerable<PatientDto> GetAllPatients()
        {
            var patients = _context
                .Patients
                .Include(p => p.Diet)
                .ToList();

            var patientsDtos = patients.Select(p => new PatientDto()
            {
                Id = p.Id,
                FirstName = p.FirstName,
                LastName = p.LastName,
                ContactEmail = p.ContactEmail,
                ContactNumber = p.ContactNumber,
                Sex = p.Sex,
                Age = p.Age,
                Weight = p.Weight,
                Height = p.Height,
                Diet = Map(p.Diet)
            });

            return patientsDtos;
        }
        public PatientDto GetPatient(int id)
        {
            var patient = GetPatientById(id);

            var patientDto = new PatientDto()
            {
                Id = id,
                FirstName = patient.FirstName,
                LastName = patient.LastName,
                ContactEmail = patient.ContactEmail,
                ContactNumber = patient.ContactNumber,
                Sex = patient.Sex,
                Age = patient.Age,
                Weight = patient.Weight,
                Height = patient.Height,
                Diet = Map(patient.Diet)
            };

            return patientDto;
        }

        public void UpdatePatient(UpdatePatientDto dto, int id)
        {
            var patient = GetPatientById(id);

            patient.Id = id;
            patient.FirstName = dto.FirstName;
            patient.LastName = dto.LastName;
            patient.ContactNumber = dto.ContactNumber;
            patient.ContactEmail = dto.ContactEmail;
            patient.Weight = dto.Weight;
            patient.Height = dto.Height;
            patient.Age = dto.Age;

            _context.SaveChanges();
        }

        public void DeletePatient(int id)
        {
            _logger.LogError("Patient with id: {Id} DELETE action invoked", id);

            var patient = GetPatientById(id);

            _context.Patients.Remove(patient);
            _context.SaveChanges();
        }

        private DietDto? Map(Diet? diet) =>
            diet is null
            ? null
            : new DietDto
            {
                Id = diet.Id,
                Name = diet.Name,
                Description = diet.Description,
                CalorificValue = diet.CalorificValue,
                ProhibitedProducts = diet.ProhibitedProducts,
                RecommendedProducts = diet.RecommendedProducts
            };

        private Patient GetPatientById(int id)
        {
            var patient = _context
                .Patients
                .Include(p => p.Diet)
                .FirstOrDefault(p => p.Id == id);

            if (patient is null)
                
                throw new NotFoundException("Patient not found");

            return patient;
        }
    }
}