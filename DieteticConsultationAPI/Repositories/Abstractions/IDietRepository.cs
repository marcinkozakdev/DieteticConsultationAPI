using DieteticConsultationAPI.Entities;

namespace DieteticConsultationAPI.Repositories.Abstractions
{
    public interface IDietRepository
    {
        IEnumerable<Diet>? GetAll();
        Diet? GetById(int? id);
        Diet? AddOrUpdate(Diet? diet);
        Diet? Delete(int? id);


    }
}
