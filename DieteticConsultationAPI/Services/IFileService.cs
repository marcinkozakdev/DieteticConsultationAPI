using DieteticConsultationAPI.Entities;
using DieteticConsultationAPI.Models;

namespace DieteticConsultationAPI.Services
{
    public interface IFileService
    {
        void UploadFile (IFormFile formFile);
        FileDto DownloadFile(int id);
        void DeleteFile(int id);

    }
}
