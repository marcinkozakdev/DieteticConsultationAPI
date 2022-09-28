using AutoMapper;
using DieteticConsultationAPI.Authorization;
using DieteticConsultationAPI.Entities;
using DieteticConsultationAPI.Exceptions;
using DieteticConsultationAPI.Models;
using DieteticConsultationAPI.Models.Pagination;
using DieteticConsultationAPI.Repositories.Abstractions;
using DieteticConsultationAPI.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using System.Linq.Expressions;

namespace DieteticConsultationAPI.Services
{
    public class PatientService : IPatientService
    {
        private readonly ILogger<PatientService> _logger;
        private readonly IAuthorizationService _authorizationService;
        private readonly IUserContextService _userContextService;
        private readonly IPatientRepository _patientRepository;

        public PatientService(ILogger<PatientService> logger, IAuthorizationService authorizationService, IUserContextService userContextService, IPatientRepository patientRepository)
        {
            _logger = logger;
            _authorizationService = authorizationService;
            _userContextService = userContextService;
            _patientRepository = patientRepository;
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

            patient.CreatedById = _userContextService.GetUserId;

            _patientRepository.AddOrUpdate(patient);

            return patient.Id;
        }

        public PagedResult<PatientDto> GetAllPatients(PatientQuery query)
        {
            var baseQuery = _patientRepository.GetAllPatientsWithDiet(query);

            if(!string.IsNullOrEmpty(query.SortBy))
            {
                var columnsSelector = new Dictionary<string, Expression<Func<Patient, object>>>
                {
                    { nameof(Patient.FirstName), p=>p.FirstName },
                    { nameof(Patient.LastName), p=>p.LastName }
                };

                var selectedColumn = columnsSelector[query.SortBy];

                baseQuery = query.SortDirection == SortDirection.ASC 
                    ? baseQuery.OrderBy(selectedColumn)
                    : baseQuery.OrderByDescending(selectedColumn);
            }

            var patients = baseQuery
                .Skip(query.PageSize * (query.PageNumber - 1))
                .Take(query.PageSize)
                .ToList();

            var totaItemsCount = baseQuery.Count();

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
            }).ToList();

            var result = new PagedResult<PatientDto>(patientsDtos, totaItemsCount , query.PageSize, query.PageNumber);

            return result;
        }
        public PatientDto GetPatient(int id)
        {
            var patient = GetPatientById(id);

            var authorizationResult = _authorizationService.AuthorizeAsync(_userContextService.User, patient, new ResourceOperationRequirement(ResourceOperation.Read)).Result;

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

            var authorizationResult = _authorizationService.AuthorizeAsync(_userContextService.User, patient, new ResourceOperationRequirement(ResourceOperation.Update)).Result;

            patient.Id = id;
            patient.FirstName = dto.FirstName;
            patient.LastName = dto.LastName;
            patient.ContactNumber = dto.ContactNumber;
            patient.ContactEmail = dto.ContactEmail;
            patient.Weight = dto.Weight;
            patient.Height = dto.Height;
            patient.Age = dto.Age;

            _patientRepository.AddOrUpdate(patient);
        }

        public void DeletePatient(int id)
        {
            _logger.LogError("Patient with id: {Id} DELETE action invoked", id);

            var patient = GetPatientById(id);

            var authorizationResult = _authorizationService.AuthorizeAsync(_userContextService.User, patient, new ResourceOperationRequirement(ResourceOperation.Delete)).Result;

            _patientRepository.Delete(id);
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
            var patient = _patientRepository.GetPatientWithDiet(id);

            if (patient is null)
                throw new NotFoundException("Patient not found");

            return patient;
        }
    }
}