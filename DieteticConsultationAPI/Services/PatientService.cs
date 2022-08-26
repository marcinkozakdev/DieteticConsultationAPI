using AutoMapper;
using DieteticConsultationAPI.Entities;
using DieteticConsultationAPI.Exceptions;
using DieteticConsultationAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace DieteticConsultationAPI.Services
{
    public interface IPatientService
    {
        PatientDto GetById(int id);
        IEnumerable<PatientDto> GetAll();
        int Add(AddPatientDto dto);
        void Delete(int id);
        void Update(UpdatePatientDto dto, int id);
    }

    public class PatientService : IPatientService
    {
        private readonly DieteticConsultationDbContext _dbContext;
        private readonly ILogger<PatientService> _logger;

        public PatientService(DieteticConsultationDbContext dbContext, ILogger<PatientService> logger)
        {
            _dbContext = dbContext;
            _logger = logger;
        }

        public void Update(UpdatePatientDto dto, int id)
        {
            var patient = _dbContext
                .Patients
                .FirstOrDefault(p => p.Id == id);

            if (patient is null)
                throw new NotFoundException("Patient not found");

            patient.FirstName = dto.FirstName;
            patient.LastName = dto.LastName;
            patient.ContactNumber = dto.ContactNumber;
            patient.ContactEmail = dto.ContactEmail;
            patient.Weight = dto.Weight;
            patient.Height = dto.Height;
            patient.Age = dto.Age;
            _dbContext.SaveChanges();
        }

        public void Delete(int id)
        {
            _logger.LogError("Patient with id: {Id} DELETE action invoked", id);

            var patient = _dbContext
                .Patients
                .FirstOrDefault(p => p.Id == id);

            if (patient is null)
                throw new NotFoundException("Patient not found");

            _dbContext.Patients.Remove(patient);
            _dbContext.SaveChanges();

        }

        public PatientDto GetById(int id)
        {
            var patient = _dbContext
                .Patients
                .Include(p => p.Diet)
                .FirstOrDefault(p => p.Id == id);

            if (patient is null)
                throw new NotFoundException("Patient not found");

            var result = new PatientDto()
            {
                FirstName = patient.FirstName,
                LastName = patient.LastName,
                ContactEmail = patient.ContactEmail,
                ContactNumber = patient.ContactNumber,
                Sex = patient.Sex,
                Age = patient.Age,
                Weight = patient.Weight,
                Height = patient.Height,
                //Diet = MapDiet(patient.Diet)

            };

            return result;
        }

        public IEnumerable<PatientDto> GetAll()
        {
            var patients = _dbContext
                .Patients
                .Include(p => p.Diet)
                .ToList();

            var patientsDtos = patients.Select(p => new PatientDto()
            {
                FirstName = p.FirstName,
                LastName = p.LastName,
                ContactEmail = p.ContactEmail,
                ContactNumber = p.ContactNumber,
                Sex = p.Sex,
                Age = p.Age,
                Weight = p.Weight,
                Height = p.Height,
                // Diet = MapDiet(p.Diet)

            });

            return patientsDtos;
        }

        public int Add(AddPatientDto dto)
        {
            var patient = new Patient
            {
                FirstName = dto.FirstName,
                LastName = dto.LastName,
                ContactEmail = dto.ContactEmail,
                ContactNumber = dto.ContactNumber,
                Sex = dto.Sex,
                Age = dto.Age,
                Weight = dto.Weight,
                Height = dto.Height,
                Diet = Map(dto.Diet!)
            };

            _dbContext.Patients.Add(patient);
            _dbContext.SaveChanges();

            return patient.Id;
        }
        
        private Diet Map(DietDto diet) =>
            diet is null
                ? throw new ArgumentNullException()
                : new()
                {
                    Name = diet.Name,
                    Description = diet.Description,
                    CalorificValue = diet.CalorificValue,
                    ProhibitedProducts = diet.ProhibitedProducts,
                    RecommendedProducts = diet.RecommendedProducts,
                };

        //private static DietDto MapDiet(Diet diet) =>
        //   new DietDto
        //   {
        //       Name = diet.Name,
        //       Description = diet.Description,
        //       CalorificValue = diet.CalorificValue,
        //       ProhibitedProducts = diet.ProhibitedProducts,
        //       RecommendedProducts = diet.RecommendedProducts,

        //   };

        //    private static DieticianDto MapDietician(Dietician dietician) =>
        //        new DieticianDto
        //        {
        //            FirstName = dietician.FirstName,
        //            LastName = dietician.LastName,
        //            Specialization = dietician.Specialization,
        //            ContactEmail = dietician.ContactEmail,
        //            ContactNumber = dietician.ContactNumber
        //        };
    }
}