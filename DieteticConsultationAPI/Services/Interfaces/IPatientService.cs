using DieteticConsultationAPI.Models;
using DieteticConsultationAPI.Models.Pagination;
using System.Security.Claims;

namespace DieteticConsultationAPI.Services.Interfaces
{
    public interface IPatientService
    {
        Task<int> Create(PatientDto dto);
        Task<PagedResult<PatientDto>> GetAll(PatientQuery query);
        Task<PatientDto> GetById(int id);
        Task Delete(int id);
        Task Update(PatientDto dto, int id);
    }
}
