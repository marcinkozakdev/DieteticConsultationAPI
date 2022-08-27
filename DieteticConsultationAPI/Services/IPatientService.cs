using DieteticConsultationAPI.Models;

namespace DieteticConsultationAPI.Services
{
    public interface IPatientService
    {
        int CreatePatient(CreatePatientDto dto);
        IEnumerable<PatientDto> GetAllPatients();
        PatientDto GetPatient(int id);
        void DeletePatient(int id);
        void UpdatePatient(UpdatePatientDto dto, int id);
    }
}
