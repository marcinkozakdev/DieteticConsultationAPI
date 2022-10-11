using DieteticConsultationAPI.Models;

namespace DieteticConsultationAPI.Services.Interfaces;

public interface IDieticianService
{
    Task<int> CreateDietician(CreateDieticianDto dto);
    Task<IEnumerable<DieticianDto>> GetAllDieticians();
    Task<DieticianDto> GetDietician(int id);
    Task DeleteDietician(int id);
}