using DieteticConsultationAPI.Entities;

namespace DieteticConsultationAPI.Repositories.Abstractions
{
    public interface IPatientRepository
    {
        IQueryable<Patient> GetAll();
        Patient? GetById(int? id);
        Patient? AddOrUpdate(Patient patient);
        void Delete(int? id);
    }
}

