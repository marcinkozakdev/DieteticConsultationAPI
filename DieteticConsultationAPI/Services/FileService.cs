using DieteticConsultationAPI.Entities;
using DieteticConsultationAPI.Exceptions;
using DieteticConsultationAPI.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Net.Http.Headers;
using System.Net.Mail;
using File = DieteticConsultationAPI.Entities.File;

namespace DieteticConsultationAPI.Services
{
    public class FileService : IFileService
    {
        private readonly DieteticConsultationDbContext _context;

        public FileService(DieteticConsultationDbContext context)
        {
            _context = context;
        }

        public void UploadFile(IFormFile file)
        {
            if (file != null && file.Length > 0)
            {
                using (var memoryStream = new MemoryStream())
                {
                    file.CopyTo(memoryStream);

                    var newFile = new File();
                    newFile.FileName = Path.GetFileName(file.FileName);
                    newFile.MimeType = file.ContentType;
                    newFile.Attachment = memoryStream.ToArray();
                    newFile.Date = DateTime.Now;

                    _context.Files.Add(newFile);
                    _context.SaveChanges();
                }
            }
        }

        public FileDto DownloadFile(int id)
        {
            var file = GetFileById(id);

            if (file == null)
                throw new NotFoundException("File not found");

            var fileDto = new FileDto()
            {
                Id = id,
                FileName = file.FileName,
                MimeType = file.MimeType,
                Attachment = file.Attachment,
                Date = file.Date,
            };

            return fileDto;
        }

        public void DeleteFile(int id)
        {
            var file = GetFileById(id);

            _context.Files.Remove(file);
            _context.SaveChanges();
        }

        private File GetFileById(int id)
        {
            var file = _context
                .Files
                .FirstOrDefault(d => d.Id == id);

            if (file == null)
                throw new NotFoundException("File not found");

            return file;
        }
    }
}

