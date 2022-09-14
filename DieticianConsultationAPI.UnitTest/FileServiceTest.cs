using DieteticConsultationAPI.Entities;
using DieteticConsultationAPI.Exceptions;
using DieteticConsultationAPI.Repositories.Abstractions;
using DieteticConsultationAPI.Services;
using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualStudio.TestPlatform.CommunicationUtilities.Resources;
using Moq;
using System.Text;
using Xunit;
using static System.Net.Mime.MediaTypeNames;

namespace DieticianConsultationAPI.UnitTest
{
    public class FileServiceTest
    {
        private readonly Mock<IFileRepository> _repositoryMock;
        private readonly FileService _sut;
        private IFormFile formFile;

        public FileServiceTest()
        {
            _repositoryMock = new Mock<IFileRepository>();
            _sut = new FileService(_repositoryMock.Object);
        }

        [Fact]
        public void UploadFile_FileIsNotNullAndFileLenghtGreaterThanZero_ReturnUploadedFile()
        {
            // arrange
         

            // act

            // assert

        }

        [Theory]
        [InlineData(6)]
        [InlineData(3)]
        public void UploadFile_BadRequest_ReturnUploadedFile(int fileLenght)
        {
            // arrange

            // act

            // assert

        }

        [Fact]
        public void DownloadFile_FileFoundById_ReturnFile()
        {
            // arrange
            int id = 1;
            FileModel file = SampleFile();
            _repositoryMock.Setup(x => x.GetById(id)).Returns(file);

            // act
            FileModelDto result = _sut.DownloadFile(file.Id);

            // assert
            _repositoryMock.Verify(x => x.GetById(file.Id), Times.Once());
            result.FileName.Should().NotBe(null);

        }

        [Theory]
        [InlineData(2)]
        [InlineData(3)]
        public void DownloadFile_FileNotFound_ReturnNotFoundException(int id)
        {
            // arrange
            FileModel file = SampleFile();
            _repositoryMock.Setup(x => x.GetById(id)).Returns(file);

            // act
            Action result = () => _sut.DownloadFile(file.Id);

            // arrange
            Assert.Throws<NotFoundException>(result);
        }

        [Fact]
        public void DeleteFile_FileFoundById_DeleteFile()
        {
            // arrange
            int id = 1;
            FileModel file = SampleFile();
            _repositoryMock.Setup(x => x.GetById(id)).Returns(file);
            _repositoryMock.Setup(x => x.Delete(It.IsAny<int>()));

            // act
            _sut.DeleteFile(file.Id);

            // assert
            _repositoryMock.Verify(x => x.Delete(file.Id), Times.Once());
        }

        [Theory]
        [InlineData(2)]
        [InlineData(3)]
        public void DeleteFile_FileNotFound_ReturnNotFoundException(int id)
        {
            // arrange
            FileModel file = SampleFile();
            _repositoryMock.Setup(x => x.GetById(id)).Returns(file);
            _repositoryMock.Setup(x => x.Delete(It.IsAny<int>()));

            // act
            Action result = () => _sut.DeleteFile(file.Id);

            // assert
            Assert.Throws<NotFoundException>(result);

        }

        private FileModel SampleFile()
        {
            UTF8Encoding enc = new UTF8Encoding();

            return new FileModel()
            {
                Id = 1,
                FileName = "Diet",
                FileType = "application/pdf",
            };
            
        }

        private FileModel SampleFileFormFile(IFormFile file)
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
            }
            return (FileModel)file;
        }

    }

   
}
