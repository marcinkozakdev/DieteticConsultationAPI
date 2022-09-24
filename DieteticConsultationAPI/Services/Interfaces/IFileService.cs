using DieteticConsultationAPI.Entities;
using DieteticConsultationAPI.Models;

namespace DieteticConsultationAPI.Services.Interfaces
{
    public interface IFileService
    {
        void UploadFile(IFormFile formFile);
        FileModelDto DownloadFile(int id);
        void DeleteFile(int id);
    }
}
