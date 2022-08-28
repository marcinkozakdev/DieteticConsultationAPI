using DieteticConsultationAPI.Models;

namespace DieteticConsultationAPI.Services
{
    public interface IDietService
    {
        int CreateDiet(CreateDietDto dto);
        IEnumerable<DietDto> GetAllDiets();
        DietDto GetDiet(int id);
        void DeleteDiet(int id);
        void UpdateDiet(UpdateDietDto dto, int id);
    }
}