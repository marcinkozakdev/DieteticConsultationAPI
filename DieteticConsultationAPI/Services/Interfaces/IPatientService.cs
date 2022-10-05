using DieteticConsultationAPI.Models;
using DieteticConsultationAPI.Models.Pagination;
using System.Security.Claims;

namespace DieteticConsultationAPI.Services.Interfaces
{
    public interface IPatientService
    {
        Task<int> CreatePatient(CreatePatientDto dto);
        Task<PagedResult<PatientDto>> GetAllPatients(PatientQuery query);
        Task<PatientDto> GetPatient(int id);
        Task DeletePatient(int id);
        Task UpdatePatient(UpdatePatientDto dto, int id);
    }
}
