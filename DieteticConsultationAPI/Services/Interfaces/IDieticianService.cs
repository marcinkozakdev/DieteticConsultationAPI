using DieteticConsultationAPI.Models;

namespace DieteticConsultationAPI.Services.Interfaces;

public interface IDieticianService
{
    Task<int> AddOrUpdateDietician(DieticianDto dto, int id);
    Task<IEnumerable<DieticianDto>> GetAllDieticians();
    Task<DieticianDto> GetDietician(int id);
    Task DeleteDietician(int id);
}