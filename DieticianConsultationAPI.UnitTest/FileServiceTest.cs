using DieteticConsultationAPI.Entities;
using DieteticConsultationAPI.Exceptions;
using DieteticConsultationAPI.Repositories.Abstractions;
using DieteticConsultationAPI.Services;
using FluentAssertions;
using Moq;
using Xunit;

namespace DieticianConsultationAPI.UnitTest
{
    public class FileServiceTest
    {
        private readonly Mock<IFileRepository> _fileRepositoryMock;

        public FileServiceTest()
        {
            _fileRepositoryMock = new Mock<IFileRepository>();
        }

        [Fact]
        public void UploadFile_FileIsNullAndFileLengthIsZero_ReturnNotFoundException()
        {
            // arrange
            var file = new FormFile(Stream.Null, 0, 0, null, null);

            // act
            var _sut = new FileService(_fileRepositoryMock.Object);
            Action result = () => _sut.UploadFile(file);

            // assert
            Assert.Throws<NotFoundException>(result);
        }

        [Fact]
        public void DownloadFile_FileFoundById_ReturnFile()
        {
            // arrange
            int id = 1;
            var file = SampleFile();

            _fileRepositoryMock
                .Setup(x => x.GetById(id))
                .Returns(file);

            // act
            var _sut = new FileService(_fileRepositoryMock.Object);
            var result = _sut.DownloadFile(file.Id);

            // assert
            _fileRepositoryMock
                .Verify(x => x.GetById(file.Id), Times.Once());

            result.FileName.Should().NotBe(null);
        }

        [Fact]
        public void DownloadFile_FileNotFound_ReturnNotFoundException()
        {
            // arrange
            int id = 2;
            var file = SampleFile();

            _fileRepositoryMock
                .Setup(x => x.GetById(id))
                .Returns(file);

            // act
            var _sut = new FileService(_fileRepositoryMock.Object);
            Action result = () => _sut.DownloadFile(file.Id);

            // arrange
            Assert.Throws<NotFoundException>(result);
        }

        [Fact]
        public void DeleteFile_FileFoundById_DeleteFile()
        {
            // arrange
            int id = 1;
            var file = SampleFile();

            _fileRepositoryMock
                .Setup(x => x.GetById(id))
                .Returns(file);

            _fileRepositoryMock
                .Setup(x => x.Delete(It.IsAny<int>()));

            // act
            var _sut = new FileService(_fileRepositoryMock.Object);
            _sut.DeleteFile(file.Id);

            // assert
            _fileRepositoryMock
                .Verify(x => x.Delete(file.Id), Times.Once());
        }

        [Fact]
        public void DeleteFile_FileNotFound_ReturnNotFoundException()
        {
            // arrange
            int id = 2;
            var file = SampleFile();

            _fileRepositoryMock
                .Setup(x => x.GetById(id))
                .Returns(file);

            _fileRepositoryMock
                .Setup(x => x.Delete(It.IsAny<int>()));

            // act
            var _sut = new FileService(_fileRepositoryMock.Object);
            Action result = () => _sut.DeleteFile(file.Id);

            // assert
            Assert.Throws<NotFoundException>(result);
        }

        private FileModel SampleFile()
        {
            return new FileModel()
            {
                Id = 1,
                FileName = "Diet",
                FileType = "application/pdf",
            };
        }
    }
}
