using DieteticConsultationAPI.Models;

namespace DieteticConsultationAPI.Services
{
    public interface IDieticianService
    {
        int CreateDietician(CreateDieticianDto dto);
        IEnumerable<DieticianDto> GetAllDieticians();
        DieticianDto GetDietician(int id);
        void UpdateDietician(UpdateDieticianDto dto, int id);
        void DeleteDietician(int id);
    }
}
