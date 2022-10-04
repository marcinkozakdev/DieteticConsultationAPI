﻿using DieteticConsultationAPI.Entities;
using DieteticConsultationAPI.Exceptions;
using DieteticConsultationAPI.Repositories.Abstractions;
using DieteticConsultationAPI.Services.Interfaces;


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
            if (file is null || file.Length == 0)
                NotFoundHttpException.For("File not found");

            using var memoryStream = new MemoryStream();

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

        public FileModelDto DownloadFile(int id)
        {
            var file = GetFileById(id);

            var fileDto = new FileModelDto()
            {
                Id = id,
                FileName = file.FileName,
                FileType = file.FileType,
                Attachment = file.Attachment,
                Date = file.Date,
            };

            return fileDto;
        }

        public void DeleteFile(int id)
        {
            var file = GetFileById(id);
            
            _fileRepository.Delete(file.Id);
        }

        private FileModel GetFileById(int id)
        {
            var file = _fileRepository.GetById(id);

            if (file is null)
                NotFoundHttpException.For("File not found");

            return file;
        }
    }
}

