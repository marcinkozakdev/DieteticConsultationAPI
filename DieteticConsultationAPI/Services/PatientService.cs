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

        public Task Create(PatientDto patientDto) =>
         _patientRepository.AddOrUpdate(Patient.For(patientDto));


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

            var patientsDtos = patients.Select(PatientDto.For).ToArray();

            var result = new PagedResult<PatientDto>(patientsDtos, totaItemsCount, query.PageSize, query.PageNumber);

            return result;
        }
        public async Task<PatientDto> GetById(int id)
        {
            var patient = PatientDto.For(await _patientRepository.GetById(id));

            if (patient is null)
                CannotFindResourceException.For(id);

            var authorizationResult = _authorizationService.AuthorizeAsync(_userContextService.User, patient, new ResourceOperationRequirement(ResourceOperation.Read)).Result;

            if (!authorizationResult.Succeeded)
                ForbiddenResourceException.For("Authorization failed");

            return patient;
        }

        public async Task Update(PatientDto patientDto)
        {
            var patient = PatientDto.For(await _patientRepository.GetById(patientDto.Id));

            if (patient is null)
                CannotFindResourceException.For(patientDto.Id);

            var authorizationResult = _authorizationService.AuthorizeAsync(_userContextService.User, patient, new ResourceOperationRequirement(ResourceOperation.Update)).Result;

            if (!authorizationResult.Succeeded)
                ForbiddenResourceException.For("Authorization failed");

            await _patientRepository.AddOrUpdate(Patient.For(patientDto));
        }

        public async Task Delete(int id)
        {
            var patient = PatientDto.For(await _patientRepository.GetById(id));

            if (patient is null)
                CannotFindResourceException.For(id);

            var authorizationResult = _authorizationService.AuthorizeAsync(_userContextService.User, patient, new ResourceOperationRequirement(ResourceOperation.Delete)).Result;

            if (!authorizationResult.Succeeded)
                ForbiddenResourceException.For("Authorization failed");

            await _patientRepository.Delete(id);
        }
    }
}