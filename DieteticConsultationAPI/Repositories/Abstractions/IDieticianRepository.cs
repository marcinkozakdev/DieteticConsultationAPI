using DieteticConsultationAPI.Entities;
using DieteticConsultationAPI.Models;

namespace DieteticConsultationAPI.Repositories.Abstractions
{
    public interface IDieticianRepository
    {
        ICollection<Dietician> GetAll();
        ICollection<Dietician> GetAllWithPatients();
        Dietician? GetById(int? id);
        Dietician? AddOrUpdate(Dietician dietician);
       void Delete(int? id);
    }
}
