using DieteticConsultationAPI.Entities;
using DieteticConsultationAPI.Models;

namespace DieteticConsultationAPI.Services.Interfaces
{
    public interface IFileService
    {
        Task UploadFile(IFormFile formFile);
        Task<FileModelDto> DownloadFile(int id);
        Task DeleteFile(int id);
    }
}
