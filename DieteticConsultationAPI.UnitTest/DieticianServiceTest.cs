using DieteticConsultationAPI.Entities;
using DieteticConsultationAPI.Exceptions;
using DieteticConsultationAPI.Models;
using DieteticConsultationAPI.Repositories.Abstractions;
using DieteticConsultationAPI.Services;
using FluentAssertions;
using Moq;
using Xunit;

namespace DieteticConsultationAPI.UnitTest
{
    public class DieticianServiceTest
    {
        private readonly Mock<IDieticianRepository> _dieticianRepositoryMock;

        public DieticianServiceTest()
        {
            _dieticianRepositoryMock = new Mock<IDieticianRepository>();
        }

        [Fact]
        public async Task GetAllDieticians_DieticiansFound_ReturnAllDieticians()
        {
            // arrange
            ICollection<Dietician> dieticians = new List<Dietician>()
            {
               SampleDietician()
            };

            _dieticianRepositoryMock
                .Setup(x => x.GetAll())
                .ReturnsAsync(dieticians);

            // act
            var _sut = new DieticianService(_dieticianRepositoryMock.Object);
            var result = await _sut.GetAll();

            // arrange
            _dieticianRepositoryMock
                .Verify(x => x.GetAll(), Times.Once());

            result.Count().Should().Be(1);
        }

        [Fact]
        public async Task GetDietician_DieticianFound_ReturnDietician()
        {
            // arrange
            int id = 1;
            var dietician = SampleDietician();

            _dieticianRepositoryMock
                .Setup(x => x.GetById(id))
                .ReturnsAsync(dietician);

            // act
            var _sut = new DieticianService(_dieticianRepositoryMock.Object);
            var result = await _sut.GetById(dietician.Id);

            // assert
            result.FirstName.Should().NotBe(null);
        }

        [Fact]
        public async Task GetDietician_DieticianNotFound_CannotFindResourceException()
        {
            // arrange
            int id = 2;
            var dietician = SampleDietician();

            _dieticianRepositoryMock
                .Setup(x => x.GetById(id))
                .ReturnsAsync(dietician);

            // act
            var _sut = new DieticianService(_dieticianRepositoryMock.Object);

            // assert

            await Assert.ThrowsAsync<CannotFindResourceException>(() => _sut.GetById(dietician.Id));
        }

        [Fact]
        public async Task GetDieticianById_DieticianFound_ReturnDieticianById()
        {
            // arrange
            int id = 1;
            var dietician = SampleDietician();

            _dieticianRepositoryMock
                .Setup(x => x.GetById(id))
                .ReturnsAsync(dietician);

            // act
            var _sut = new DieticianService(_dieticianRepositoryMock.Object);
            var result = await _sut.GetById(dietician.Id);

            // assert
            _dieticianRepositoryMock
                .Verify(x => x.GetById(dietician.Id), Times.Once());
        }

        [Fact]
        public async Task GetDieticianById_DieticianNotFound_CannotFindResourceException()
        {
            // arrange
            int id = 2;
            var dietician = SampleDietician();

            _dieticianRepositoryMock
                .Setup(x => x.GetById(id))
                .ReturnsAsync(dietician);

            // act
            var _sut = new DieticianService(_dieticianRepositoryMock.Object);

            // assert
            await Assert.ThrowsAsync<CannotFindResourceException>(() => _sut.GetById(dietician.Id));
        }

        [Fact]
        public async Task CreateDietician_InputDieticianData_ReturnDietician()
        {
            // arrange
            var dietician = new DieticianDto()
            {
                Id = 2,
                FirstName = "Test FirstName",
                LastName = "Test LastName",
                Specialization = "Dietician",
                ContactEmail = "test@test.com",
                ContactNumber = "111222333",
                Patients = new List<PatientDto>()
            };

            _dieticianRepositoryMock
                .Setup(x => x.AddOrUpdate(It.IsAny<Dietician>()));

            // act
            var _sut = new DieticianService(_dieticianRepositoryMock.Object);
            await _sut.Create(dietician);

            // assert
            _dieticianRepositoryMock
                .Verify(x => x.AddOrUpdate(It.IsAny<Dietician>()), Times.Once());

            Assert.Equal("Test FirstName", dietician.FirstName);
            Assert.Equal(2, dietician.Id);
        }

        [Fact]
        public async Task UpdateDietician_DieticianFound_ReturnDietician()
        {
            // arrange
            int id = 1;
            var dietician = SampleDietician();

            DieticianDto updateDietician = new DieticianDto()
            {
                FirstName = "Test FirstName",
                LastName = "Test LastName",
                Specialization = "Dietician",
                ContactEmail = "test@test.com",
                ContactNumber = "111222333",
                Patients = new List<PatientDto>()
            };

            _dieticianRepositoryMock
                .Setup(x => x.GetById(id))
                .ReturnsAsync(dietician);

            _dieticianRepositoryMock
                .Setup(x => x.AddOrUpdate(It.IsAny<Dietician>()));

            // act
            var _sut = new DieticianService(_dieticianRepositoryMock.Object);
            await _sut.Update(updateDietician);

            // assert
            _dieticianRepositoryMock
                .Verify(x => x.AddOrUpdate(It.IsAny<Dietician>()), Times.Once());

            Assert.Equal("Test LastName", updateDietician.LastName);
        }

        [Fact]
        public async Task UpdateDietician_DieticianNotFound_CreateDietician()
        {
            // arrange
            int id = 2;
            var dietician = SampleDietician();

            DieticianDto createDietician = new DieticianDto()
            {
                FirstName = "Test FirstName",
                LastName = "Test LastName",
                Specialization = "Dietician",
                ContactEmail = "test@test.com",
                ContactNumber = "111222333",
                Patients= new List<PatientDto>()
            };

            _dieticianRepositoryMock
                .Setup(x => x.GetById(id))
                .ReturnsAsync(dietician);

            _dieticianRepositoryMock
                .Setup(x => x.AddOrUpdate(dietician));

            // act
            var _sut = new DieticianService(_dieticianRepositoryMock.Object);
            await _sut.Create(createDietician);

            // assert
            Assert.Equal("Test LastName", createDietician.LastName);
        }

        [Fact]
        public async Task DeleteDietician_DieticianFound_ReturnNull()
        {
            // arrange
            var dietician = SampleDietician();

            await Task.FromResult(_dieticianRepositoryMock
                .Setup(x => x.GetById(dietician.Id))
                .ReturnsAsync(dietician));

            await Task.FromResult(_dieticianRepositoryMock
                .Setup(x => x.Delete(It.IsAny<int>())));

            // act
            var _sut = new DieticianService(_dieticianRepositoryMock.Object);
            await _sut.Delete(dietician.Id);

            // assert
            _dieticianRepositoryMock.Verify(x => x.Delete(dietician.Id), Times.Once);
        }

        private Dietician SampleDietician()
        {
            return new Dietician()
            {
                Id = 1,
                FirstName = "Test FirstName",
                LastName = "Test LastName",
                Specialization = "Test Specialization",
                ContactEmail = "test1@test.com",
                ContactNumber = "111222333",
                Patients = new List<Patient>()
            };
        }
    }
}
