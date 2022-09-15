using DieteticConsultationAPI.Entities;
using DieteticConsultationAPI.Exceptions;
using DieteticConsultationAPI.Models;
using DieteticConsultationAPI.Repositories.Abstractions;
using DieteticConsultationAPI.Services;
using DieteticConsultationAPI.Services.Interfaces;
using FluentAssertions;
using Microsoft.AspNetCore.Authorization;
using Moq;
using Xunit;

namespace DieticianConsultationAPI.UnitTest
{
    public class PatientServiceTest
    {
        private readonly Mock<IPatientRepository> _repositoryMock;
        private readonly Mock<ILogger<PatientService>> _loggerMock;
        private readonly Mock<IAuthorizationService> _authorizationServiceMock;
        private readonly Mock<IUserContextService> _userContextServiceMock;
        private readonly PatientService _sut;

        public PatientServiceTest()
        {
            _repositoryMock = new Mock<IPatientRepository>();
            _loggerMock = new Mock<ILogger<PatientService>>();
            _authorizationServiceMock = new Mock<IAuthorizationService>();
            _userContextServiceMock = new Mock<IUserContextService>();
            _sut = new PatientService(_loggerMock.Object, _authorizationServiceMock.Object, _userContextServiceMock.Object, _repositoryMock.Object);
        }

        [Fact]
        public void GetAllPatients_PatientsFound_ReturnAllPatients()
        {
            // arrange

            // act

            // assert
        }

        [Fact]
        public void GetPatient_PatientFound_ReturnPatient()
        {
            // arrange
            int id = 1;
            Patient patient = SamplePatient();
            _repositoryMock.Setup(x=>x.GetById(id)).Returns(patient);

            // act
            PatientDto result = _sut.GetPatient(patient.Id);

            // assert
            result.FirstName.Should().NotBe(null);
        }


        [Theory]
        [InlineData(2)]
        [InlineData(3)]
        [InlineData(4)]
        public void GetPatient_PatientNotFound_ReturnNotFoundException(int id)
        {
            // arrange
            Patient patient = SamplePatient();
            _repositoryMock.Setup(x => x.GetById(id)).Returns(patient);

            // act
            Action result = () => _sut.GetPatient(patient.Id);

            // assert
            Assert.Throws<NotFoundException>(result);
        }

        [Fact]
        public void CreatePatient_InputPatientData_ReturnPatient()
        {
            // arrange
            var patient = new CreatePatientDto()
            {
                FirstName = "Marcin",
                LastName = "Kozak",
                ContactEmail = "marcinkozak@test.com",
                ContactNumber = "999888777",
                Sex = "Male",
                Weight = 60,
                Height = 168,
                Age = 28,
                Id = 2,
                DieticianId = 1
            };

            _repositoryMock.Setup(x => x.AddOrUpdate(It.IsAny<Patient>()));

            // act
            var result = _sut.CreatePatient(patient);

            // assert
            _repositoryMock.Verify(x => x.AddOrUpdate(It.IsAny<Patient>()), Times.Once());
            result.Should().NotBe(null);
            Assert.Equal("Marcin", patient.FirstName);
        }

        [Fact]
        public void UpdatePatient_PatientFound_ReturnPatient()
        {
            // arrange
            int id = 1;
            Patient patient = SamplePatient();
            UpdatePatientDto updatePatient = new UpdatePatientDto()
            {
                FirstName = "Marcin",
                LastName = "Kozak",
                ContactEmail = "marcinkozak@test.com",
                ContactNumber = "999888777",
                Sex = "Male",
                Weight = 65,
                Height = 175,
                Age = 29,
            };
            _repositoryMock.Setup(x => x.GetById(id)).Returns(patient);
            _repositoryMock.Setup(x => x.AddOrUpdate(It.IsAny<Patient>())).Returns(patient);

            // act
            _sut.UpdatePatient(updatePatient, patient.Id);

            // assert
            _repositoryMock.Verify(x => x.AddOrUpdate(It.IsAny<Patient>()), Times.Once());
            Assert.Equal(65, updatePatient.Weight);
            Assert.Equal(175, updatePatient.Height);
            Assert.Equal(29, updatePatient.Age);
        }

        [Theory]
        [InlineData(2)]
        [InlineData(3)]
        [InlineData(4)]
        public void UpdatePatient_PatientNotFound_ReturnNotFoundException(int id)
        {
            // arrange
            Patient patient = SamplePatient();
            UpdatePatientDto updatePatient = new UpdatePatientDto()
            {
                FirstName = "Marcin",
                LastName = "Kozak",
                ContactEmail = "marcinkozak@test.com",
                ContactNumber = "999888777",
                Sex = "Male",
                Weight = 65,
                Height = 175,
                Age = 29,
            };
            _repositoryMock.Setup(x => x.GetById(id)).Returns(patient);
            _repositoryMock.Setup(x => x.AddOrUpdate(patient)).Returns(patient);

            // act
            Action result = () => _sut.UpdatePatient(updatePatient, patient.Id);

            // assert
            Assert.Throws<NotFoundException>(result);
        }

        [Fact]
        public void DeletePatient_PatientFound_ReturnNull()
        {
            // arrange
            int id = 1;
            Patient patient = SamplePatient();
            _repositoryMock.Setup(x => x.GetById(id)).Returns(patient);
            _repositoryMock.Setup(x => x.Delete(It.IsAny<int>()));
            
            // act
            _sut.DeletePatient(patient.Id);

            // assert
            _repositoryMock.Verify(x => x.Delete(patient.Id), Times.Once);
        }

        [Theory]
        [InlineData(2)]
        [InlineData(3)]
        [InlineData(4)]
        public void DeletePatient_PatientNotFound_ReturnNotFoundException(int id)
        {
            // arrange
            Patient patient = SamplePatient();
            _repositoryMock.Setup(x => x.GetById(id)).Returns(patient);
            _repositoryMock.Setup(x => x.Delete(It.IsAny<int>()));

            // act
            Action result = () => _sut.DeletePatient(patient.Id);

            // assert
            Assert.Throws<NotFoundException>(result);
        }

            private Patient SamplePatient()
        {
            return new Patient()
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
            };
        }
    }
}








