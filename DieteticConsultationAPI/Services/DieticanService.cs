using AutoMapper;
using DieteticConsultationAPI.Entities;
using DieteticConsultationAPI.Exceptions;
using DieteticConsultationAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace DieteticConsultationAPI.Services
{
    public interface IDieticianService
    {
        int Add(AddDieticianDto dto);
        IEnumerable<DieticianDto> GetAll();
        DieticianDto GetById(int id);
        void Delete(int id);
        void Update(int id, UpdateDieticianDto dto);
    }

    public class DieticianService : IDieticianService
    {
        private readonly DieteticConsultationDbContext _dbContext;
        private readonly ILogger<DieticianService> _logger;

        public DieticianService(DieteticConsultationDbContext dbContext, ILogger<DieticianService> logger)
        {
            _dbContext = dbContext;
            _logger = logger;
        }
       
        
        public void Update(int id, UpdateDieticianDto dto)
        {
            var dietician = _dbContext
               .Dieticians
               .FirstOrDefault(d => d.Id == id);

            if (dietician == null)
                throw new NotFoundException("Dietician not found");

            dietician.FirstName = dto.FirstName;
            dietician.LastName = dto.LastName;
            dietician.Specialization = dto.Specialization;
            dietician.ContactEmail = dto.ContactEmail;
            dietician.ContactNumber = dto.ContactNumber;

            _dbContext.SaveChanges();
        }
        
        public void Delete(int id)
        {
            _logger.LogWarning($"Dietician with id: {id} DELETE action invoked");

            var dietician = _dbContext
               .Dieticians
               .FirstOrDefault(d => d.Id == id);

            if (dietician == null)
                throw new NotFoundException("Dietician not found");

            _dbContext.Dieticians.Remove(dietician);
            _dbContext.SaveChanges();
        }


        public DieticianDto GetById(int id)
        {
            var dietician = _dbContext
                .Dieticians
                .Include(d => d.Patients)
                .FirstOrDefault(d => d.Id == id);

            if (dietician == null)
                throw new NotFoundException("Dietician not found");

            var result = new DieticianDto()
            {
                Id = id,
                FirstName = dietician.FirstName,
                LastName = dietician.LastName,
                Specialization = dietician.Specialization,
                ContactEmail = dietician.ContactEmail,
                ContactNumber = dietician.ContactNumber,
                Patients = dietician.Patients.Select(Map).ToList()
            };

            return result;
        }

        public IEnumerable<DieticianDto> GetAll()
        {
            var dieticians = _dbContext
                            .Dieticians
                            .Include(d => d.Patients)
                            .ToList();

            var dieticiansDtos = dieticians.Select(d => new DieticianDto()
            {
                Id = d.Id,
                FirstName = d.FirstName,
                LastName = d.LastName,
                Specialization = d.Specialization,
                ContactEmail = d.ContactEmail,
                ContactNumber = d.ContactNumber,
                Patients = d.Patients.Select(Map).ToList()
            });
            return dieticiansDtos;
        }

        public int Add(AddDieticianDto dto)
        {
            var dietician = new Dietician()
            {
                FirstName = dto.FirstName,
                LastName = dto.LastName,
                Specialization = dto.Specialization,
                ContactEmail = dto.ContactEmail,
                ContactNumber = dto.ContactNumber
            };

            _dbContext.Dieticians.Add(dietician);
            _dbContext.SaveChanges();

            return dietician.Id;
        }

        private static PatientDto Map(Patient patient) =>
            new PatientDto
            {
                FirstName = patient.FirstName,
                LastName = patient.LastName,
                ContactEmail = patient.ContactEmail,
                ContactNumber = patient.ContactNumber,
                Sex = patient.Sex,
                Weight = patient.Weight,
                Height = patient.Height,
                Age = patient.Age
            };






    }
}
