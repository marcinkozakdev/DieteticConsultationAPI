using DieteticConsultationAPI.Models;

namespace DieteticConsultationAPI.Services.Interfaces;

public interface IDieticianService
{
    Task Create(DieticianDto dto);
    Task<ICollection<DieticianDto>> GetAll();
    Task<DieticianDto> GetById(int id);
    Task Delete(int id);
    Task Update(DieticianDto dto);
}