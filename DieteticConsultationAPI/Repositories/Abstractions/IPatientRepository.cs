using DieteticConsultationAPI.Entities;

namespace DieteticConsultationAPI.Repositories.Abstractions
{
    public interface IPatientRepository
    {
        ICollection<Patient> GetAll();
        ICollection<Patient> GetAllWithDiet();
        Patient? GetById(int? id);
        Patient? AddOrUpdate(Patient patient);
        void Delete(int? id);
    }
}

