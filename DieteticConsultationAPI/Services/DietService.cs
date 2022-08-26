using DieteticConsultationAPI.Entities;
using DieteticConsultationAPI.Exceptions;
using DieteticConsultationAPI.Models;

namespace DieteticConsultationAPI.Services;

public class DietService : IDietService
{
    private readonly DieteticConsultationDbContext _context;
    public DietService(DieteticConsultationDbContext context) => _context = context;

    public async Task<int> Create(CreateDietDto dto)
    {
        if (dto is null)
            throw new ArgumentNullException(nameof(dto));

        var patient = _context.Patients.FirstOrDefault(p => p.Id == dto.PatientId);

        if (patient is null)
            throw new NotFoundException("Patient not found");

        var dietEntity = new Diet
        {
            Name = dto.Name,
            Description = dto.Description,
            CalorificValue = dto.CalorificValue,
            ProhibitedProducts = dto.ProhibitedProducts,
            RecommendedProducts = dto.RecommendedProducts
        };

        _context.Diets.Add(dietEntity);
        _context.SaveChangesAsync();

        return dietEntity.Id;
    }
}