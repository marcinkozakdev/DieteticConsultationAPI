using DieteticConsultationAPI.Entities;
using DieteticConsultationAPI.Exceptions;
using DieteticConsultationAPI.Models;
using DieteticConsultationAPI.Models.Pagination;
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
        private readonly Mock<IPatientRepository> _patientRepositoryMock;
        private readonly Mock<ILogger<PatientService>> _loggerMock;
        private readonly Mock<IAuthorizationService> _authorizationServiceMock;
        private readonly Mock<IUserContextService> _userContextServiceMock;

        public PatientServiceTest()
        {
            _patientRepositoryMock = new Mock<IPatientRepository>();
            _loggerMock = new Mock<ILogger<PatientService>>();
            _authorizationServiceMock = new Mock<IAuthorizationService>();
            _userContextServiceMock = new Mock<IUserContextService>();
        }

        [Fact]
        public void GetAllPatients_QueryIsNullOrEmpty_ReturnAllPatients()
        {
            // arrange
            var patients = new List<Patient>()
            {
               SamplePatient(),
               SamplePatient_2()
            };

            var query = new PatientQuery()
            {
                PageNumber = 1,
                PageSize = 10,
                SearchPhrase = null,
                SortBy = null,
            };

            _patientRepositoryMock
                .Setup(x => x.GetAll(query))
                .Returns((IQueryable<Patient>)patients.AsQueryable());

            // act
            var _sut = new PatientService(_loggerMock.Object, _authorizationServiceMock.Object, _userContextServiceMock.Object, _patientRepositoryMock.Object);

            var result = _sut.GetAllPatients(query);

            // arrange
            _patientRepositoryMock
                .Verify(x => x.GetAll(query), Times.Once());
            result.TotalItemsCount.Should().Be(2);
        }

        [Fact]
        public void GetPatient_PatientFound_ReturnPatient()
        {
            // arrange
            int id = 1;
            var patient = SamplePatient();

            _patientRepositoryMock
                .Setup(x => x.GetById(id))
                .Returns(patient);

            // act
            var _sut = new PatientService(_loggerMock.Object, _authorizationServiceMock.Object, _userContextServiceMock.Object, _patientRepositoryMock.Object);

            var result = _sut.GetPatient(patient.Id);

            // assert
            result.FirstName.Should().NotBe(null);
        }

        [Fact]
        public void GetPatient_PatientNotFound_ReturnNotFoundException()
        {
            // arrange
            int id = 2;
            var patient = SamplePatient();

            _patientRepositoryMock
                .Setup(x => x.GetById(id))
                .Returns(patient);

            // act
            var _sut = new PatientService(_loggerMock.Object, _authorizationServiceMock.Object, _userContextServiceMock.Object, _patientRepositoryMock.Object);

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

            _patientRepositoryMock
                .Setup(x => x.AddOrUpdate(It.IsAny<Patient>()));

            // act
            var _sut = new PatientService(_loggerMock.Object, _authorizationServiceMock.Object, _userContextServiceMock.Object, _patientRepositoryMock.Object);

            var result = _sut.CreatePatient(patient);

            // assert
            result.Should().NotBe(null);
            Assert.Equal("Marcin", patient.FirstName);
        }

        [Fact]
        public void UpdatePatient_PatientFound_ReturnPatient()
        {
            // arrange
            int id = 1;
            var patient = SamplePatient();
            var updatePatient = new UpdatePatientDto()

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

            _patientRepositoryMock
                .Setup(x => x.GetById(id))
                .Returns(patient);

            _patientRepositoryMock
                .Setup(x => x.AddOrUpdate(It.IsAny<Patient>()))
                .Returns(patient);

            // act
            var _sut = new PatientService(_loggerMock.Object, _authorizationServiceMock.Object, _userContextServiceMock.Object, _patientRepositoryMock.Object);

            _sut.UpdatePatient(updatePatient, patient.Id);

            // assert
            Assert.Equal(65, updatePatient.Weight);
            Assert.Equal(175, updatePatient.Height);
            Assert.Equal(29, updatePatient.Age);
        }

        [Fact]
        public void UpdatePatient_PatientNotFound_ReturnNotFoundException()
        {
            // arrange
            int id = 2;
            var patient = SamplePatient();
            var updatePatient = new UpdatePatientDto()

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

            _patientRepositoryMock
                .Setup(x => x.GetById(id))
                .Returns(patient);

            _patientRepositoryMock
                .Setup(x => x.AddOrUpdate(patient))
                .Returns(patient);

            // act
            var _sut = new PatientService(_loggerMock.Object, _authorizationServiceMock.Object, _userContextServiceMock.Object, _patientRepositoryMock.Object);

            Action result = () => _sut.UpdatePatient(updatePatient, patient.Id);

            // assert
            Assert.Throws<NotFoundException>(result);
        }

        [Fact]
        public void DeletePatient_PatientFound_ReturnNull()
        {
            // arrange
            int id = 1;
            var patient = SamplePatient();

            _patientRepositoryMock
                .Setup(x => x.GetById(id))
                .Returns(patient);
            _patientRepositoryMock
                .Setup(x => x.Delete(It.IsAny<int>()));

            // act
            var _sut = new PatientService(_loggerMock.Object, _authorizationServiceMock.Object, _userContextServiceMock.Object, _patientRepositoryMock.Object);

            _sut.DeletePatient(patient.Id);

            // assert
            _patientRepositoryMock
                .Verify(x => x.Delete(patient.Id), Times.Once);
        }

        [Fact]
        public void DeletePatient_PatientNotFound_ReturnNotFoundException()
        {
            // arrange
            int id = 2;
            var patient = SamplePatient();

            _patientRepositoryMock
                .Setup(x => x.GetById(id))
                .Returns(patient);

            _patientRepositoryMock
                .Setup(x => x.Delete(It.IsAny<int>()));

            // act
            var _sut = new PatientService(_loggerMock.Object, _authorizationServiceMock.Object, _userContextServiceMock.Object, _patientRepositoryMock.Object);

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
        private Patient SamplePatient_2()
        {
            return new Patient()
            {
                FirstName = "Dominika",
                LastName = "Kozak",
                ContactEmail = "marcinkozak@test.com",
                ContactNumber = "999888777",
                Sex = "Female",
                Weight = 55,
                Height = 155,
                Age = 26,
                Id = 2,
                DieticianId = 1
            };
        }
    }
}








