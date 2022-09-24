using DieteticConsultationAPI.Entities;

namespace DieteticConsultationAPI.Repositories.Abstractions
{
    public interface IFileRepository
    {
        void Upload(FileModel? file);
        FileModel GetById(int? id);
        void Delete(int? id);
    }
}
