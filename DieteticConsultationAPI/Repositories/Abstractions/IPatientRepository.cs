using DieteticConsultationAPI.Entities;
using DieteticConsultationAPI.Models.Pagination;

namespace DieteticConsultationAPI.Repositories.Abstractions
{
    public interface IPatientRepository
    {
        Task<IQueryable<Patient>> GetAll(PatientQuery query);
        Task<Patient> GetById(int id);
        Task AddOrUpdate(Patient patient);
        Task Delete(int id);
    }
}

