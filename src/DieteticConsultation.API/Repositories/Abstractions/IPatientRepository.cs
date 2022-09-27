using DieteticConsultationAPI.Entities;
using DieteticConsultationAPI.Models.Pagination;

namespace DieteticConsultationAPI.Repositories.Abstractions
{
    public interface IPatientRepository
    {
        IQueryable<Patient> GetAll(PatientQuery query);
        Patient? GetById(int? id);
        Patient? AddOrUpdate(Patient patient);
        void Delete(int? id);
    }
}

