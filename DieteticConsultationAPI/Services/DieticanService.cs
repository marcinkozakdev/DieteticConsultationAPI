﻿using DieteticConsultationAPI.Entities;
using DieteticConsultationAPI.Exceptions;
using DieteticConsultationAPI.Models;
using DieteticConsultationAPI.Repositories.Abstractions;
using DieteticConsultationAPI.Services.Interfaces;


namespace DieteticConsultationAPI.Services
{
    public class DieticianService : IDieticianService
    {
        private readonly ILogger _logger;
        private readonly IDieticianRepository _dieticianRepository;

        public DieticianService(ILogger<DieticianService> logger, IDieticianRepository dieticianRepository)
        {
            _logger = logger;
            _dieticianRepository = dieticianRepository;
        }
        public int CreateDietician(CreateDieticianDto dto)
        {
            var dietician = new Dietician()
            {
                FirstName = dto.FirstName,
                LastName = dto.LastName,
                Specialization = dto.Specialization,
                ContactEmail = dto.ContactEmail,
                ContactNumber = dto.ContactNumber
            };

            _dieticianRepository.AddOrUpdate(dietician);

            return dietician.Id;
        }

        public IEnumerable<DieticianDto> GetAllDieticians()
        {
            var dieticians = _dieticianRepository.GetAll();

            if (dieticians is null)
                throw new NotFoundException("The diet list is empty");

            var dieticiansDtos = dieticians.Select(d => new DieticianDto()
            {
                Id = d.Id,
                FirstName = d.FirstName,
                LastName = d.LastName,
                Specialization = d.Specialization,
                ContactEmail = d.ContactEmail,
                ContactNumber = d.ContactNumber,
                Patients = d.Patients.Select(Map).ToList(),
            }).ToList();

            return dieticiansDtos;
        }

        public DieticianDto GetDietician(int id)
        {
            var dietician = GetDieticianById(id);

            var dieticianDto = new DieticianDto()
            {
                Id = id,
                FirstName = dietician.FirstName,
                LastName = dietician.LastName,
                Specialization = dietician.Specialization,
                ContactEmail = dietician.ContactEmail,
                ContactNumber = dietician.ContactNumber,
                Patients = dietician.Patients.Select(Map).ToList()
            };

            return dieticianDto;
        }

        public void UpdateDietician(UpdateDieticianDto dto, int id)
        {
            var dietician = GetDieticianById(id);

            dietician.FirstName = dto.FirstName;
            dietician.LastName = dto.LastName;
            dietician.Specialization = dto.Specialization;
            dietician.ContactEmail = dto.ContactEmail;
            dietician.ContactNumber = dto.ContactNumber;

            _dieticianRepository.AddOrUpdate(dietician);
        }

        public void DeleteDietician(int id)
        {
            _logger.LogWarning($"Dietician with id: {id} DELETE action invoked");

            var dietician = GetDieticianById(id);

            _dieticianRepository.Delete(dietician.Id);
        }

        public Dietician GetDieticianById(int id)
        {
            var dietician = _dieticianRepository.GetById(id);

            if (dietician is null)
                throw new NotFoundException("Dietician not found");

            return dietician;
        }

        private PatientDto Map(Patient patient) =>
            new PatientDto
            {
                Id = patient.Id,
                FirstName = patient.FirstName,
                LastName = patient.LastName,
                ContactEmail = patient.ContactEmail,
                ContactNumber = patient.ContactNumber,
                Sex = patient.Sex,
                Weight = patient.Weight,
                Height = patient.Height,
                Age = patient.Age,
                Diet = Map(patient.Diet)
            };

        private static DietDto? Map(Diet? diet) =>
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
    }
}
