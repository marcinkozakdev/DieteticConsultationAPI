using DieteticConsultationAPI.Entities;

namespace DieteticConsultationAPI.Repositories.Abstractions
{
    public interface IDieticianRepository
    {
        Task<IEnumerable<Dietician>> GetAll();
        Task<Dietician> GetById(int? id);
        Task<Dietician> AddOrUpdate(Dietician dietician);
        Task Delete(int? id);
    }
}
