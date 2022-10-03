using DieteticConsultationAPI.Entities;

namespace DieteticConsultationAPI.Repositories.Abstractions
{
    public interface IDietRepository
    {
        IEnumerable<Diet>? GetAllDietsWithPatientsAndFiles();
        Diet? GetDietWithPatientAndFiles(int? id);
        Diet? AddOrUpdate(Diet? diet);
        void Delete(int? id);
    }
}
