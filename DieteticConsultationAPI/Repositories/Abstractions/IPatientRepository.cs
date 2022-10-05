using DieteticConsultationAPI.Entities;
using DieteticConsultationAPI.Models.Pagination;

namespace DieteticConsultationAPI.Repositories.Abstractions
{
    public interface IPatientRepository
    {
        Task<IQueryable<Patient>> GetAllPatientsWithDiet(PatientQuery query);
        Task<Patient> GetPatientWithDiet(int? id);
        Task<Patient> AddOrUpdate(Patient patient);
        Task Delete(int? id);
    }
}

