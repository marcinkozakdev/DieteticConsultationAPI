using DieteticConsultationAPI.Entities;
using DieteticConsultationAPI.Exceptions;
using DieteticConsultationAPI.Repositories.Abstractions;
using DieteticConsultationAPI.Services;
using FluentAssertions;
using Moq;
using Xunit;

namespace DieteticConsultationAPI.UnitTest
{
    public class FileServiceTest
    {
        private readonly Mock<IFileRepository> _fileRepositoryMock;

        public FileServiceTest()
        {
            _fileRepositoryMock = new Mock<IFileRepository>();
        }

        [Fact]
        public async Task UploadFile_FileIsNullAndFileLengthIsZero_ReturnCannotFindResourceException()
        {
            // arrange
            var file = new FormFile(Stream.Null, 0, 0, null, null);

            // act
            var _sut = new FileService(_fileRepositoryMock.Object);

            // assert
            await Assert.ThrowsAsync<CannotFindResourceException>(() => _sut.UploadFile(file));
        }

        [Fact]
        public async Task DownloadFile_FileFoundById_ReturnFile()
        {
            // arrange
            int id = 1;
            var file = SampleFile();

            _fileRepositoryMock
                .Setup(x => x.GetById(id))
                .ReturnsAsync(file);

            // act
            var _sut = new FileService(_fileRepositoryMock.Object);
            var result = await _sut.DownloadFile(file.Id);

            // assert
            _fileRepositoryMock
                .Verify(x => x.GetById(file.Id), Times.Once());

            await Task.FromResult(result.FileName.Should().NotBe(null));
        }

        [Fact]
        public async Task DownloadFile_FileNotFound_ReturnCannotFindResourceException()
        {
            // arrange
            int id = 2;
            var file = SampleFile();

            _fileRepositoryMock
                .Setup(x => x.GetById(id))
                .ReturnsAsync(file);

            // act
            var _sut = new FileService(_fileRepositoryMock.Object);

            // arrange
            await Assert.ThrowsAsync<CannotFindResourceException>(() => _sut.DownloadFile(file.Id));
        }

        [Fact]
        public async Task DeleteFile_FileFoundById_DeleteFile()
        {
            // arrange
            int id = 1;
            var file = SampleFile();

            _fileRepositoryMock
                .Setup(x => x.GetById(id))
                .ReturnsAsync(file);

            _fileRepositoryMock
                .Setup(x => x.Delete(It.IsAny<int>()));

            // act
            var _sut = new FileService(_fileRepositoryMock.Object);
            await _sut.DeleteFile(file.Id);

            // assert
            _fileRepositoryMock
                .Verify(x => x.Delete(file.Id), Times.Once());
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
