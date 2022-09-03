using DieteticConsultationAPI.Models;
using DieteticConsultationAPI.Models.Pagination;
using System.Security.Claims;

namespace DieteticConsultationAPI.Services
{
    public interface IPatientService
    {
        int CreatePatient(CreatePatientDto dto);
        PagedResult<PatientDto> GetAllPatients(PatientQuery query);
        PatientDto GetPatient(int id);
        void DeletePatient(int id);
        void UpdatePatient(UpdatePatientDto dto, int id);
    }
}
