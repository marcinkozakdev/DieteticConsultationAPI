using DieteticConsultationAPI.Entities;

namespace DieteticConsultationAPI.Repositories.Abstractions
{
    public interface IDieticianRepository
    {
        Task<ICollection<Dietician>> GetAll();
        Task<Dietician> GetById(int id);
        Task AddOrUpdate(Dietician dietician);
        Task Delete(int id);
    }
}
