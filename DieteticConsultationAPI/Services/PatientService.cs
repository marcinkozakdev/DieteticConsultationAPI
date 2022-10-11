using AutoMapper;
using DieteticConsultationAPI.Authorization;
using DieteticConsultationAPI.Entities;
using DieteticConsultationAPI.Exceptions;
using DieteticConsultationAPI.Models;
using DieteticConsultationAPI.Models.Pagination;
using DieteticConsultationAPI.Repositories;
using DieteticConsultationAPI.Repositories.Abstractions;
using DieteticConsultationAPI.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using System.Linq.Expressions;

namespace DieteticConsultationAPI.Services
{
    public class PatientService : IPatientService
    {
        private readonly IAuthorizationService _authorizationService;
        private readonly IUserContextService _userContextService;
        private readonly IPatientRepository _patientRepository;

        public PatientService(IAuthorizationService authorizationService, IUserContextService userContextService, IPatientRepository patientRepository)
        {
            _authorizationService = authorizationService;
            _userContextService = userContextService;
            _patientRepository = patientRepository;
        }

        public Task CreatePatient(PatientDto patientDto) =>
         _patientRepository.AddOrUpdate(Patient.For(patientDto));
        }

        public async Task<PagedResult<PatientDto>> GetAll(PatientQuery query)
        {
        var baseQuery = await _patientRepository.GetAll(query);

            if (!string.IsNullOrEmpty(query.SortBy))
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

            var result = new PagedResult<PatientDto>(patientsDtos, totaItemsCount, query.PageSize, query.PageNumber);

            return result;
        }
        public async Task<PatientDto> GetById(int id)
        {
            var patient = await GetPatientById(id);

            var authorizationResult = _authorizationService.AuthorizeAsync(_userContextService.User, patient, new ResourceOperationRequirement(ResourceOperation.Read)).Result;

            if (!authorizationResult.Succeeded)
                ForbidHttpException.For("Authorization failed");

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

        public async Task Update(UpdatePatientDto dto, int id)
        {
            var patient = await GetPatientById(id);

            var authorizationResult = _authorizationService.AuthorizeAsync(_userContextService.User, patient, new ResourceOperationRequirement(ResourceOperation.Update)).Result;

            if (!authorizationResult.Succeeded)
                ForbidHttpException.For("Authorization failed");

            patient.Id = id;
            patient.FirstName = dto.FirstName;
            patient.LastName = dto.LastName;
            patient.ContactNumber = dto.ContactNumber;
            patient.ContactEmail = dto.ContactEmail;
            patient.Weight = dto.Weight;
            patient.Height = dto.Height;
            patient.Age = dto.Age;

            await _patientRepository.AddOrUpdate(patient);
        }

        public async Task Delete(int id)
        {
        var authorizationResult = _authorizationService.AuthorizeAsync(_userContextService.User, patient, new ResourceOperationRequirement(ResourceOperation.Delete)).Result;

            if (!authorizationResult.Succeeded)
                ForbidHttpException.For("Authorization failed");

            await _patientRepository.Delete(id);
        }

        private DietDto Map(Diet diet) =>
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

        private async Task<Patient> GetPatientById(int id)
        {
            var patient = await _patientRepository.GetById(id);

            if (patient is null)
                NotFoundHttpException.For("Patient not found");

            return patient;
        }
    }
}