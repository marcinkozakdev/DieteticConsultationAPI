using DieteticConsultationAPI.Models;

namespace DieteticConsultationAPI.Services
{
    public interface IDieticianService
    {
        int CreateDietician(CreateDieticianDto dto);
        IEnumerable<DieticianDto> GetAllDieticians();
        DieticianDto GetDietician(int id);
        void UpdateDietician(int id, UpdateDieticianDto dto);
        void DeleteDietician(int id);
    }
}
