using DieteticConsultationAPI.Entities;
using DieteticConsultationAPI.Models;

namespace DieteticConsultationAPI.Repositories.Abstractions
{
    public interface IDieticianRepository
    {
        IEnumerable<Dietician> GetAll();
        Dietician? GetById(int? id);
        Dietician? AddOrUpdate(Dietician dietician);
        void Delete(int? id);
    }
}
