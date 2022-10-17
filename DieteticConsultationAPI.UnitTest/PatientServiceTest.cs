using DieteticConsultationAPI.Authorization;
using DieteticConsultationAPI.Entities;
using DieteticConsultationAPI.Exceptions;
using DieteticConsultationAPI.Models;
using DieteticConsultationAPI.Models.Pagination;
using DieteticConsultationAPI.Repositories.Abstractions;
using DieteticConsultationAPI.Services;
using DieteticConsultationAPI.Services.Interfaces;
using FluentAssertions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.IdentityModel.Tokens;
using Moq;
using System.Security.Claims;
using Xunit;

namespace DieteticConsultationAPI.UnitTest
{
    public class PatientServiceTest
    {
        private readonly Mock<IPatientRepository> _patientRepositoryMock;
        private readonly Mock<IAuthorizationService> _authorizationServiceMock;
        private readonly Mock<IUserContextService> _userContextServiceMock;

        public PatientServiceTest()
        {
            _patientRepositoryMock = new Mock<IPatientRepository>();
            _authorizationServiceMock = new Mock<IAuthorizationService>();
            _userContextServiceMock = new Mock<IUserContextService>();
        }

        [Fact]
        public async Task GetAllPatients_QueryIsNullOrEmpty_ReturnAllPatients()
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
                .ReturnsAsync((IQueryable<Patient>)patients.AsQueryable());

            // act
            var _sut = new PatientService(_authorizationServiceMock.Object, _userContextServiceMock.Object, _patientRepositoryMock.Object);

            var result = await _sut.GetAll(query);

            // arrange
            _patientRepositoryMock
                .Verify(x => x.GetAll(query), Times.Once());
            result.TotalItemsCount.Should().Be(2);
            result.TotalPages.Should().Be(1);
            result.ItemFrom.Should().Be(1);
            result.ItemsTo.Should().Be(10);
        }

        [Fact]
        public async Task GetPatient_PatientFoundAndAuthorizationSuccess_ReturnPatient()
        {
            // arrange
            int id = 1;
            var patient = SamplePatient();

            _patientRepositoryMock
                .Setup(x => x.GetById(id))
                .ReturnsAsync(patient);

            _authorizationServiceMock
                .Setup(x => x.AuthorizeAsync(
                    It.IsAny<ClaimsPrincipal>(),
                    It.IsAny<object>(),
                    It.IsAny<IEnumerable<IAuthorizationRequirement>>()))
                .ReturnsAsync(AuthorizationResult.Success);

            // act
            var _sut = new PatientService(_authorizationServiceMock.Object, _userContextServiceMock.Object, _patientRepositoryMock.Object);

            var result = await _sut.GetById(patient.Id);

            // assert
            result.FirstName.Should().NotBe(null);
        }

        [Fact]
        public async Task GetPatient_PatientFoundAndAuthorizationFailed_ReturnForbidException()
        {
            // arrange
            int id = 1;
            var patient = SamplePatient();

            _patientRepositoryMock
                .Setup(x => x.GetById(id))
                .ReturnsAsync(patient);

            _authorizationServiceMock
                .Setup(x => x.AuthorizeAsync(
                    It.IsAny<ClaimsPrincipal>(),
                    It.IsAny<object>(),
                    It.IsAny<IEnumerable<IAuthorizationRequirement>>()))
                .ReturnsAsync(AuthorizationResult.Failed);

            // act
            var _sut = new PatientService(_authorizationServiceMock.Object, _userContextServiceMock.Object, _patientRepositoryMock.Object);

            // assert

            await Assert.ThrowsAsync<ForbiddenResourceException>(() => _sut.GetById(patient.Id));
        }

        [Fact]
        public async Task GetPatient_PatientNotFound_ReturnNotFoundException()
        {
            // arrange
            int id = 2;
            var patient = SamplePatient();

            _patientRepositoryMock
                .Setup(x => x.GetById(id))
                .ReturnsAsync(patient);

            // act
            var _sut = new PatientService(_authorizationServiceMock.Object, _userContextServiceMock.Object, _patientRepositoryMock.Object);

            // assert
            await Assert.ThrowsAsync<CannotFindResourceException>(() => _sut.GetById(patient.Id));
        }

        [Fact]
        public async Task CreatePatient_InputPatientData_ReturnPatient()
        {
            // arrange
            var patient = new PatientDto()
            {
                FirstName = "Test FirstName",
                LastName = "Test LastName",
                ContactEmail = "test@test.com",
                ContactNumber = "999888777",
                Sex = "Male",
                Weight = 60,
                Height = 168,
                Age = 28,
                Diet = new DietDto()
            };

            _patientRepositoryMock
                .Setup(x => x.AddOrUpdate(It.IsAny<Patient>()));

            // act
            var _sut = new PatientService(_authorizationServiceMock.Object, _userContextServiceMock.Object, _patientRepositoryMock.Object);

            await _sut.Create(patient);

            // assert
            Assert.Equal("Test FirstName", patient.FirstName);
        }

        [Fact]
        public async Task UpdatePatient_PatientFoundAndAuthorizationSuccess_ReturnPatient()
        {
            // arrange
            int id = 1;
            var patient = SamplePatient();
            var updatePatient = new PatientDto()

            {
                Id = 1,
                FirstName = "Test FirstName",
                LastName = "Test LastName",
                ContactEmail = "test@test.com",
                ContactNumber = "999888777",
                Sex = "Male",
                Weight = 65,
                Height = 175,
                Age = 29,
                Diet = new DietDto(),
            };

            _patientRepositoryMock
                .Setup(x => x.GetById(patient.Id))
                .ReturnsAsync(patient);

            _authorizationServiceMock
                .Setup(x => x.AuthorizeAsync(
                    It.IsAny<ClaimsPrincipal>(),
                    It.IsAny<object>(),
                    It.IsAny<IEnumerable<IAuthorizationRequirement>>()))
                .ReturnsAsync(AuthorizationResult.Success);

            _patientRepositoryMock
                .Setup(x => x.AddOrUpdate(It.IsAny<Patient>()));

            // act
            var _sut = new PatientService(_authorizationServiceMock.Object, _userContextServiceMock.Object, _patientRepositoryMock.Object);

            await _sut.Update(updatePatient);

            // assert
            Assert.Equal(65, updatePatient.Weight);
            Assert.Equal(175, updatePatient.Height);
            Assert.Equal(29, updatePatient.Age);
        }

        [Fact]
        public async Task UpdatePatient_PatientFoundAndAuthorizationFailed_ReturnForbidException()
        {
            // arrange
            int id = 1;
            var patient = SamplePatient();
            var updatePatient = new PatientDto()

            {
                Id = 1,
                FirstName = "Test FirstName",
                LastName = "Test LastName",
                ContactEmail = "test@test.com",
                ContactNumber = "999888777",
                Sex = "Male",
                Weight = 65,
                Height = 175,
                Age = 29,
                Diet = new DietDto(),
            };

            _patientRepositoryMock
                .Setup(x => x.GetById(id))
                .ReturnsAsync(patient);

            _authorizationServiceMock
                .Setup(x => x.AuthorizeAsync(
                    It.IsAny<ClaimsPrincipal>(),
                    It.IsAny<object>(),
                    It.IsAny<IEnumerable<IAuthorizationRequirement>>()))
                .ReturnsAsync(AuthorizationResult.Failed);

            _patientRepositoryMock
                .Setup(x => x.AddOrUpdate(It.IsAny<Patient>()));

            // act
            var _sut = new PatientService(_authorizationServiceMock.Object, _userContextServiceMock.Object, _patientRepositoryMock.Object);

            // assert
            await Assert.ThrowsAsync<ForbiddenResourceException>(() => _sut.Update(updatePatient));
        }

        [Fact]
        public async Task UpdatePatient_PatientNotFound_ReturnNotFoundException()
        {
            // arrange
            int id = 2;
            var patient = SamplePatient();

            var updatePatient = new PatientDto()
            {
                FirstName = "Test FirstName",
                LastName = "Test LastName",
                ContactEmail = "test@test.com",
                ContactNumber = "999888777",
                Sex = "Male",
                Weight = 65,
                Height = 175,
                Age = 29,
                Diet = new DietDto(),
            };

            _patientRepositoryMock
                .Setup(x => x.GetById(id))
                .ReturnsAsync(patient);

            _patientRepositoryMock
                .Setup(x => x.AddOrUpdate(patient));

            // act
            var _sut = new PatientService(_authorizationServiceMock.Object, _userContextServiceMock.Object, _patientRepositoryMock.Object);

            // assert
            await Assert.ThrowsAsync<CannotFindResourceException>(() => _sut.Update(updatePatient));
        }


        [Fact]
        public async Task DeletePatient_PatientFoundAndAuthorizationSuccess_ReturnNull()
        {
            // arrange
            int id = 1;
            var patient = SamplePatient();

            _patientRepositoryMock
                .Setup(x => x.GetById(patient.Id))
                .ReturnsAsync(patient);

            _authorizationServiceMock
                .Setup(x => x.AuthorizeAsync(
                    It.IsAny<ClaimsPrincipal>(),
                    It.IsAny<object>(),
                    It.IsAny<IEnumerable<IAuthorizationRequirement>>()))
                .ReturnsAsync(AuthorizationResult.Success);

            _patientRepositoryMock
                .Setup(x => x.Delete(It.IsAny<int>()));

            // act
            var _sut = new PatientService(_authorizationServiceMock.Object, _userContextServiceMock.Object, _patientRepositoryMock.Object);

            await _sut.Delete(patient.Id);

            // assert
            _patientRepositoryMock
                .Verify(x => x.Delete(patient.Id), Times.Once);
        }

        [Fact]
        public async Task DeletePatient_PatientFoundAndAuthorizationFailed_ReturnForbidException()
        {
            // arrange
            int id = 1;
            var patient = SamplePatient();

            _patientRepositoryMock
                .Setup(x => x.GetById(id))
                .ReturnsAsync(patient);

            _authorizationServiceMock
                .Setup(x => x.AuthorizeAsync(
                    It.IsAny<ClaimsPrincipal>(),
                    It.IsAny<object>(),
                    It.IsAny<IEnumerable<IAuthorizationRequirement>>()))
                .ReturnsAsync(AuthorizationResult.Failed);

            _patientRepositoryMock
                .Setup(x => x.Delete(It.IsAny<int>()));

            // act
            var _sut = new PatientService(_authorizationServiceMock.Object, _userContextServiceMock.Object, _patientRepositoryMock.Object);

            // assert
            await Assert.ThrowsAsync<ForbiddenResourceException>(() => _sut.Delete(patient.Id));
        }

        [Fact]
        public async Task DeletePatient_PatientNotFound_ReturnNotFoundException()
        {
            // arrange
            int id = 2;
            var patient = SamplePatient();

            _patientRepositoryMock
                .Setup(x => x.GetById(id))
                .ReturnsAsync(patient);

            _patientRepositoryMock
                .Setup(x => x.Delete(It.IsAny<int>()));

            // act
            var _sut = new PatientService(_authorizationServiceMock.Object, _userContextServiceMock.Object, _patientRepositoryMock.Object);

            // assert
            await Assert.ThrowsAsync<CannotFindResourceException>(() => _sut.Delete(patient.Id));
        }

        private Patient SamplePatient()
        {
            return new Patient()
            {
                Id = 1,
                FirstName = "Test FirstName",
                LastName = "Test LastName",
                ContactEmail = "test1@test.com",
                ContactNumber = "999888777",
                Sex = "Male",
                Weight = 60,
                Height = 168,
                Age = 28,
                Diet = new Diet(),
                DieticianId = 1,
            };
        }
        private Patient SamplePatient_2()
        {
            return new Patient()
            {
                Id = 1,
                FirstName = "Test FirstName2",
                LastName = "Test LastName2",
                ContactEmail = "test2@test.com",
                ContactNumber = "999888777",
                Sex = "Female",
                Weight = 55,
                Height = 155,
                Age = 26,
                Diet = new Diet(),
            };
        }
    }
}








