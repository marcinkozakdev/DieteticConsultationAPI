using DieteticConsultationAPI.Entities;
using DieteticConsultationAPI.Models;
using DieteticConsultationAPI.Models.Pagination;
using System.Security.Claims;

namespace DieteticConsultationAPI.Services.Interfaces
{
    public interface IDieticianService
    {
        int CreateDietician(CreateDieticianDto dto);
        PagedResult<DieticianDto> GetAllDieticians(DieticianQuery query);
        DieticianDto GetDietician(int id);

        void UpdateDietician(UpdateDieticianDto dto, int id);
        void DeleteDietician(int id);
    }
}
