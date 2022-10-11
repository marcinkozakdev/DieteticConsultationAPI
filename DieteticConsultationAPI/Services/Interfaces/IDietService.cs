using DieteticConsultationAPI.Models;

namespace DieteticConsultationAPI.Services.Interfaces
{
    public interface IDietService
    {
        Task Create(DietDto dto);
        Task<ICollection<DietDto>> GetAll();
        Task<DietDto> GetById(int id);
        Task Delete(int id);
        Task Update(DietDto dto);
    }
}