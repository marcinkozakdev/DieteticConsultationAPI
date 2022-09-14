using DieteticConsultationAPI.Entities;
using DieteticConsultationAPI.Exceptions;
using DieteticConsultationAPI.Models;
using DieteticConsultationAPI.Repositories.Abstractions;
using DieteticConsultationAPI.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Server.IIS.Core;
using Microsoft.EntityFrameworkCore;
using System.Collections;
using System.Net.Http.Headers;
using System.Net.Mail;
using FileModel = DieteticConsultationAPI.Entities.FileModel;

namespace DieteticConsultationAPI.Services
{
    public class FileService : IFileService
    {
        private readonly IFileRepository _fileRepository;

        public FileService(IFileRepository fileRepository)
        {
            _fileRepository = fileRepository;
        }

        public void UploadFile(IFormFile file)
        {
            if (file != null && file.Length > 0)
            {
                using (var memoryStream = new MemoryStream())
                {
                    file.CopyTo(memoryStream);

                    var newFile = new FileModel()
                    {
                        FileName = Path.GetFileName(file.FileName),
                        FileType = file.ContentType,
                        Attachment = memoryStream.ToArray(),
                        Date = DateTime.UtcNow,
                    };

                    _fileRepository.Upload(newFile);
            }
        }
            else
                throw new Exception("Bad Request");
    }

    public FileModelDto DownloadFile(int id)
    {
        var file = GetFileById(id);

        if (file == null)
            throw new NotFoundException("File not found");

            //using var stream = new MemoryStream(file.Attachment);

            var fileDto = new FileModelDto()
        {
            Id = id,
            FileName = file.FileName,
            FileType = file.FileType,
            Attachment = file.Attachment,//stream,
            Date = file.Date,
        };

        return fileDto;
    }

    public void DeleteFile(int id)
    {
        var file = GetFileById(id);

            //if(System.IO.File.Exists(file.FileName))
            //{
            //    System.IO.File.Delete(file.FileName);
            //}

            _fileRepository.Delete(file.Id);
    }

    private FileModel GetFileById(int id)
    {
        var file = _fileRepository.GetById(id);

        if (file == null)
            throw new NotFoundException("File not found");

        return file;
    }

}
}

