using DieteticConsultationAPI.Entities;
using DieteticConsultationAPI.Models.Pagination;

namespace DieteticConsultationAPI.Repositories.Abstractions
{
    public interface IPatientRepository
    {
        IQueryable<Patient> GetAllPatientsWithDiet(PatientQuery query);
        Patient? GetPatientWithDiet(int? id);
        Patient? AddOrUpdate(Patient patient);
        void Delete(int? id);
    }
}

