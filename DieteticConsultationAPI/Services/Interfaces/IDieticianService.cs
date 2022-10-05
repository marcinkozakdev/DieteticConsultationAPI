using DieteticConsultationAPI.Entities;
using DieteticConsultationAPI.Models;
using DieteticConsultationAPI.Models.Pagination;
using System.Security.Claims;

namespace DieteticConsultationAPI.Services.Interfaces
{
    public interface IDieticianService
    {
        Task<int> CreateDietician(CreateDieticianDto dto);
        Task<IEnumerable<DieticianDto>> GetAllDieticians();
        Task<DieticianDto> GetDietician(int id);
        Task UpdateDietician(UpdateDieticianDto dto, int id);
        Task DeleteDietician(int id);
    }
}
