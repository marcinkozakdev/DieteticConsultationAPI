using DieteticConsultationAPI.Models;
namespace DieteticConsultationAPI.Services;

public interface IDietService
{
    Task<int> Create(CreateDietDto dto);
    Task<DietDto> GetDiet(int id);
}