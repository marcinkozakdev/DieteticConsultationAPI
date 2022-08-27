using DieteticConsultationAPI.Entities;
using DieteticConsultationAPI.Exceptions;
using DieteticConsultationAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace DieteticConsultationAPI.Services;

public class DietService : IDietService
{
    private readonly DieteticConsultationDbContext _context;
    public DietService(DieteticConsultationDbContext context) => _context = context;

    public int Create(CreateDietDto dto)
    {
        if (dto is null)
            throw new ArgumentNullException(nameof(dto));

        var dietEntity = new Diet
        {
            Id = dto.Id,
            Name = dto.Name,
            Description = dto.Description,
            CalorificValue = dto.CalorificValue,
            ProhibitedProducts = dto.ProhibitedProducts,
            RecommendedProducts = dto.RecommendedProducts,
            PatientId = dto.PatientId
        };

        _context.Diets.Add(dietEntity);
        _context.SaveChanges();

        return dietEntity.Id;
    }

    public IEnumerable<DietDto> GetAll()
    {
        var diets = _context
            .Diets
            .Include(d => d.Patient)
            .ToList();

        var results = diets.Select(d => new DietDto()
        {
            Id = d.Id,
            Name = d.Name,
            Description = d.Description,
            CalorificValue = d.CalorificValue,
            ProhibitedProducts = d.ProhibitedProducts,
            RecommendedProducts = d.RecommendedProducts,
            
        });

        return results;
    }

    public DietDto Get(int patientId)
    {
        var diet = _context
            .Diets
            .Include(d => d.Patient)
            .ToList();

        if (diet == null)
            throw new NotFoundException("Diet not found");

        var result = new DietDto()
        {
           
        };

        return result;

    }

    //public Task<DietDto> GetDiet(int id)
    //{
    //    throw new NotImplementedException();
    //}
}