using DieteticConsultationAPI.Entities;

namespace DieteticConsultationAPI.Repositories.Abstractions
{
    public interface IFileRepository
    {
        Task Upload(FileModel file);
        Task<FileModel> GetById(int? id);
        Task Delete(int? id);
    }
}
