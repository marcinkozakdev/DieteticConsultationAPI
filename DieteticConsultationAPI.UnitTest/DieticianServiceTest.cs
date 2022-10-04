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
        private readonly Mock<ILogger<DieticianService>> _loggerMock;

        public DieticianServiceTest()
        {
            _dieticianRepositoryMock = new Mock<IDieticianRepository>();
            _loggerMock = new Mock<ILogger<DieticianService>>();
        }

        [Fact]
        public void GetAllDieticians_DieticiansFound_ReturnAllDieticians()
        {
            // arrange
            List<Dietician> dieticians = new List<Dietician>()
            {
               SampleDietician()
            };

            _dieticianRepositoryMock
                .Setup(x => x.GetAll())
                .Returns(dieticians);

            // act
            var _sut = new DieticianService(_loggerMock.Object, _dieticianRepositoryMock.Object);
            var result = _sut.GetAllDieticians();

            // arrange
            _dieticianRepositoryMock
                .Verify(x => x.GetAll(), Times.Once());

            result.Count().Should().Be(1);
        }

        [Fact]
        public void GetDietician_DieticianFound_ReturnDietician()
        {
            // arrange
            int id = 1;
            var dietician = SampleDietician();
            
            _dieticianRepositoryMock
                .Setup(x => x.GetById(id))
                .Returns(dietician);

            // act
            var _sut = new DieticianService(_loggerMock.Object, _dieticianRepositoryMock.Object);
            var result = _sut.GetDietician(dietician.Id);

            // assert
            result.FirstName.Should().NotBe(null);
        }

        [Fact]
        public void GetDietician_DieticianNotFound_ReturnNotFoundException()
        {
            // arrange
            int id = 2;
            var dietician = SampleDietician();

            _dieticianRepositoryMock
                .Setup(x => x.GetById(id))
                .Returns(dietician);

            // act
            var _sut = new DieticianService(_loggerMock.Object, _dieticianRepositoryMock.Object);
            Action result = () => _sut.GetDietician(dietician.Id);

            // assert
            Assert.Throws<NotFoundHttpException>(result);
        }

        [Fact]
        public void GetDieticianById_DieticianFound_ReturnDieticianById()
        {
            // arrange
            int id = 1;
            var dietician = SampleDietician();

            _dieticianRepositoryMock
                .Setup(x => x.GetById(id))
                .Returns(dietician);

            // act
            var _sut = new DieticianService(_loggerMock.Object, _dieticianRepositoryMock.Object);
            var result = _sut.GetDieticianById(dietician.Id);

            // assert
            _dieticianRepositoryMock
                .Verify(x => x.GetById(dietician.Id), Times.Once());
        }

        [Fact]
        public void GetDieticianById_DieticianNotFound_ReturnNotFoundException()
        {
            // arrange
            int id = 2;
            var dietician = SampleDietician();

            _dieticianRepositoryMock
                .Setup(x => x.GetById(id))
                .Returns(dietician);

            // act
            var _sut = new DieticianService(_loggerMock.Object, _dieticianRepositoryMock.Object);
            Action result = () => _sut.GetDieticianById(dietician.Id);

            // assert
            Assert.Throws<NotFoundHttpException>(result);
        }

        [Fact]
        public void CreateDietician_InputDieticianData_ReturnDietician()
        {
            // arrange
            var dietician = new CreateDieticianDto()
            {
                FirstName = "Dominika",
                LastName = "Kozak",
                Specialization = "Dietician",
                ContactEmail = "dominika.kozak@test.com",
                ContactNumber = "111222333",
                Id = 2,
            };

            _dieticianRepositoryMock
                .Setup(x => x.AddOrUpdate(It.IsAny<Dietician>()));

            // act
            var _sut = new DieticianService(_loggerMock.Object, _dieticianRepositoryMock.Object);
            var result = _sut.CreateDietician(dietician);

            // assert
            _dieticianRepositoryMock
                .Verify(x => x.AddOrUpdate(It.IsAny<Dietician>()), Times.Once());

            Assert.Equal("Dominika", dietician.FirstName);
            Assert.Equal(2, dietician.Id);
        }

        [Fact]
        public void UpdateDietician_DieticianFound_ReturnDietician()
        {
            // arrange
            int id = 1;
            var dietician = SampleDietician();

            UpdateDieticianDto updateDietician = new UpdateDieticianDto()
            {
                FirstName = "Dominika",
                LastName = "Olszowy",
                Specialization = "Dietician",
                ContactEmail = "dominika.olszowy@test.com",
                ContactNumber = "111222333",
            };

            _dieticianRepositoryMock
                .Setup(x => x.GetById(id))
                .Returns(dietician);

            _dieticianRepositoryMock
                .Setup(x => x.AddOrUpdate(It.IsAny<Dietician>()))
                .Returns(dietician);

            // act
            var _sut = new DieticianService(_loggerMock.Object, _dieticianRepositoryMock.Object);
            _sut.UpdateDietician(updateDietician, dietician.Id);

            // assert
            _dieticianRepositoryMock
                .Verify(x => x.AddOrUpdate(It.IsAny<Dietician>()), Times.Once());

            Assert.Equal("Olszowy", updateDietician.LastName);
        }

        [Fact]
        public void UpdateDietician_DieticianNotFound_ReturnNotFoundException()
        {
            // arrange
            int id = 2;
            var dietician = SampleDietician();

            UpdateDieticianDto updateDietician = new UpdateDieticianDto()
            {
                FirstName = "Dominika",
                LastName = "Olszowy",
                Specialization = "Dietician",
                ContactEmail = "dominika.olszowy@test.com",
                ContactNumber = "111222333",
            };

            _dieticianRepositoryMock
                .Setup(x => x.GetById(id))
                .Returns(dietician);

            _dieticianRepositoryMock
                .Setup(x => x.AddOrUpdate(dietician))
                .Returns(dietician);

            // act
            var _sut = new DieticianService(_loggerMock.Object, _dieticianRepositoryMock.Object);
            Action result = () => _sut.UpdateDietician(updateDietician, dietician.Id);

            // assert
            Assert.Throws<NotFoundHttpException>(result);
        }

        [Fact]
        public void DeleteDietician_DieticianFound_ReturnNull()
        {
            // arrange
            int id = 1;
            var dietician = SampleDietician();

            _dieticianRepositoryMock
                .Setup(x => x.GetById(id))
                .Returns(dietician);

            _dieticianRepositoryMock
                .Setup(x => x.Delete(It.IsAny<int>()));

            // act
            var _sut = new DieticianService(_loggerMock.Object, _dieticianRepositoryMock.Object);
            _sut.DeleteDietician(dietician.Id);

            // assert
            _dieticianRepositoryMock.Verify(x => x.Delete(dietician.Id), Times.Once);
        }

        [Theory]
        [InlineData(2)]
        [InlineData(4)]
        public void DeleteDietician_DieticianNotFound_ReturnNotFoundException(int id)
        {
            // arrange
            var dietician = SampleDietician();

            _dieticianRepositoryMock
                .Setup(x => x.GetById(id))
                .Returns(dietician);

            _dieticianRepositoryMock
                .Setup(x => x.Delete(It.IsAny<int>()));

            // act
            var _sut = new DieticianService(_loggerMock.Object, _dieticianRepositoryMock.Object);
            Action result = () => _sut.DeleteDietician(dietician.Id);

            // assert
            Assert.Throws<NotFoundHttpException>(result);
        }

        private Dietician SampleDietician()
        {
            return new Dietician()
            {
                FirstName = "Dominika",
                LastName = "Kozak",
                Specialization = "Dietician",
                ContactEmail = "dominika.kozak@test.com",
                ContactNumber = "111222333",
                Id = 1,
                Patients = new List<Patient>()
                {
                    new Patient()
                    {
                        FirstName = "Marcin",
                        LastName = "Kozak",
                        ContactEmail = "marcinkozak@test.com",
                        ContactNumber = "999888777",
                        Sex = "Male",
                        Weight = 60,
                        Height = 168,
                        Age = 28,
                        Id = 1,
                        DieticianId = 1
                    }
                }
            };
        }
    }
}
