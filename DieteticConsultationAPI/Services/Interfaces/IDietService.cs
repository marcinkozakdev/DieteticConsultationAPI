using DieteticConsultationAPI.Models;

namespace DieteticConsultationAPI.Services.Interfaces
{
    public interface IDietService
    {
        Task<int> CreateDiet(DietDto dto);
        Task<IEnumerable<DietDto>> GetAllDiets();
        Task<DietDto> GetDiet(int id);
        Task DeleteDiet(int id);
        Task UpdateDiet(DietDto dto, int id);
    }
}