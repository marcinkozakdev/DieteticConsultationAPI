using DieteticConsultationAPI.Entities;

namespace DieteticConsultationAPI.Repositories.Abstractions
{
    public interface IDietRepository
    {
        Task<IEnumerable<Diet>> GetAllDietsWithPatientsAndFiles();
        Task<Diet> GetDietWithPatientAndFiles(int id);
        Task<Diet> AddOrUpdate(Diet diet);
        Task Delete(int id);
    }
}
