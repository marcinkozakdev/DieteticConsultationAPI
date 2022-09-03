using DieteticConsultationAPI.Authorization;
using DieteticConsultationAPI.Entities;
using DieteticConsultationAPI.Exceptions;
using DieteticConsultationAPI.Models;
using DieteticConsultationAPI.Models.Pagination;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using System.Security.Claims;

namespace DieteticConsultationAPI.Services
{
    public class DieticianService : IDieticianService
    {
        private readonly DieteticConsultationDbContext _context;
        private readonly ILogger _logger;
        

        public DieticianService(DieteticConsultationDbContext context, ILogger<DieticianService> logger )
        {
            _context = context;
            _logger = logger;
          
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

            _context.Dieticians.Add(dietician);
            _context.SaveChanges();

            return dietician.Id;
        }

        public PagedResult<DieticianDto> GetAllDieticians(DieticianQuery query)
        {
            var baseQuery = _context
                            .Dieticians
                            .Include(d => d.Patients)
                            .ThenInclude(d => d.Diet)
                            .ThenInclude(d => d.Files)
                            .Where(r => query.SearchPhrase == null 
                                || r.FirstName.ToLower().Contains(query.SearchPhrase.ToLower())
                                || r.LastName.ToLower().Contains(query.SearchPhrase.ToLower()));

            if (!string.IsNullOrEmpty(query.SortBy))
            {
                var columnsSelector = new Dictionary<string, Expression<Func<Dietician, object>>>
                {
                    { nameof(Dietician.FirstName), p=>p.FirstName },
                    { nameof(Dietician.LastName), p=>p.LastName }
                };

                var selectedColumn = columnsSelector[query.SortBy];

                baseQuery = query.SortDirection == SortDirection.ASC
                    ? baseQuery.OrderBy(selectedColumn)
                    : baseQuery.OrderByDescending(selectedColumn);
            }

            var dieticians = baseQuery
                            .Skip(query.PageSize * (query.PageNumber - 1))
                            .Take(query.PageSize)
                            .ToList();

            var totaItemsCount = baseQuery.Count();

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

            var result = new PagedResult<DieticianDto>(dieticiansDtos, totaItemsCount , query.PageSize, query.PageNumber);

            return result;
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

            _context.SaveChanges();
        }

        public void DeleteDietician(int id)
        {
            _logger.LogWarning($"Dietician with id: {id} DELETE action invoked");

            var dietician = GetDieticianById(id);

            _context.Dieticians.Remove(dietician);
            _context.SaveChanges();
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

        private Dietician GetDieticianById(int id)
        {
            var dietician = _context
               .Dieticians
               .Include(d => d.Patients)
               .ThenInclude(d => d.Diet)
               .FirstOrDefault(d => d.Id == id);

            if (dietician is null)
                throw new NotFoundException("Dietician not found");

            return dietician;
        }
    }
}
