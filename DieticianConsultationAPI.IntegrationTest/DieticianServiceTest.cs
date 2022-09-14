using AutoFixture;
using DieteticConsultationAPI.Entities;
using DieteticConsultationAPI.Exceptions;
using DieteticConsultationAPI.Models;
using DieteticConsultationAPI.Repositories;
using DieteticConsultationAPI.Repositories.Abstractions;
using DieteticConsultationAPI.Services;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.TestPlatform.CommunicationUtilities;
using Moq;
using Xunit;

namespace DieticianConsultationAPI.UnitTest
{
    public class DieticianServiceTest
    {

        private readonly Mock<IDieticianRepository> _repositoryMock;
        private readonly Mock<ILogger<DieticianService>> _loggerMock;
        private readonly DieticianService _sut;

        public DieticianServiceTest()
        {
            _repositoryMock = new Mock<IDieticianRepository>();
            _loggerMock = new Mock<ILogger<DieticianService>>();
            _sut = new DieticianService(null, _loggerMock.Object, _repositoryMock.Object);
        }


        //[Fact]
        //public void GetAllDieticians_ShouldReturnAllDieticians()
        //{
        //    // arrange
        //    var getAllDieticians = new List<Dietician>()
        //    {
        //        new Dietician()
        //        {
        //            FirstName = "Mateusz"
        //        }
        //    };

        //    var loggerMock = new Mock<ILogger<DieticianService>>();
        //    var repositoryMock = new Mock<IDieticianRepository>();
        //    repositoryMock.Setup(x => x.GetAll()).Returns(getAllDieticians);

        //    //act
        //    var sut = new DieticianService(null, loggerMock.Object, repositoryMock.Object);
        //    var result = sut.GetAll();

        //    //assert
        //    result.Count.Should().Be(1);
        //    result.Single().FirstName.Should().NotBe(null);
        //}

        [Fact]
        public void GetAllDieticians_DieticiansFound_ReturnAllDieticians()
        {
            // arrange
            List<Dietician> dieticians = new List<Dietician>()
            {
               SampleDietician()
            };

            _repositoryMock.Setup(x => x.GetAll()).Returns(dieticians);

            // act

            // assert
        }

        [Fact]
        public void GetDietician_DieticianFound_ReturnDietician()
        {
            // arrange
            int id = 1;
            Dietician dietician = SampleDietician();
            _repositoryMock.Setup(x => x.GetById(id)).Returns(dietician);

            // act
            DieticianDto result = _sut.GetDietician(dietician.Id);

            // assert
            result.FirstName.Should().NotBe(null);
        }

        [Theory]
        [InlineData(2)]
        [InlineData(3)]
        [InlineData(4)]
        public void GetDietician_DieticianNotFound_ReturnNotFoundException(int id)
        {
            // arrange
            Dietician dietician = SampleDietician();
            _repositoryMock.Setup(x => x.GetById(id)).Returns(dietician);

            // act
            Action result = () =>  _sut.GetDietician(dietician.Id);

            // assert
            Assert.Throws<NotFoundException>(result);
        }

        [Fact]
        public void GetDieticianById_DieticianFound_ReturnDieticianById()
        {
            // arrange
            int id = 1;
            Dietician dietician = SampleDietician();
            _repositoryMock.Setup(x => x.GetById(id)).Returns(dietician);

            // act
            Dietician result = _sut.GetDieticianById(dietician.Id);

            // assert
            _repositoryMock.Verify(x => x.GetById(dietician.Id), Times.Once());
        }

        [Theory]
        [InlineData(2)]
        [InlineData(3)]
        [InlineData(4)]
        public void GetDieticianById_DieticianNotFound_ReturnNotFoundException(int id)
        {
            // arrange
            Dietician dietician = SampleDietician();
            _repositoryMock.Setup(x => x.GetById(id)).Returns(dietician);

            // act
            Action result = () => _sut.GetDieticianById(dietician.Id);

            // assert
            Assert.Throws<NotFoundException>(result);
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
            
            _repositoryMock.Setup(x => x.AddOrUpdate(It.IsAny<Dietician>()));

            // act
            var result = _sut.CreateDietician(dietician);

            // assert
            _repositoryMock.Verify(x => x.AddOrUpdate(It.IsAny<Dietician>()), Times.Once());
            result.Should().NotBe(null);
            Assert.Equal("Dominika", dietician.FirstName);
        }
        
        [Fact]
        public void UpdateDietician_DieticianFound_ReturnDietician()
        {
            // arrange
            int id = 1 ;
            Dietician dietician = SampleDietician();
            UpdateDieticianDto updateDietician = new UpdateDieticianDto()
            {
                FirstName = "Dominika",
                LastName = "Olszowy",
                Specialization = "Dietician",
                ContactEmail = "dominika.olszowy@test.com",
                ContactNumber = "111222333",
            };
            _repositoryMock.Setup(x => x.GetById(id)).Returns(dietician);
            _repositoryMock.Setup(x => x.AddOrUpdate(It.IsAny<Dietician>())).Returns(dietician);

            // act
            _sut.UpdateDietician(updateDietician, dietician.Id);

            // assert
            _repositoryMock.Verify(x => x.AddOrUpdate(It.IsAny<Dietician>()), Times.Once());
            Assert.Equal("Olszowy", updateDietician.LastName);
        }

        [Theory]
        [InlineData(2)]
        [InlineData(3)]
        [InlineData(4)]
        public void UpdateDietician_DieticianNotFound_ReturnNotFoundException(int id)
        {
            // arrange
            Dietician dietician = SampleDietician();
            UpdateDieticianDto updateDietician = new UpdateDieticianDto()
            {
                FirstName = "Dominika",
                LastName = "Olszowy",
                Specialization = "Dietician",
                ContactEmail = "dominika.olszowy@test.com",
                ContactNumber = "111222333",
            };
            _repositoryMock.Setup(x => x.GetById(id)).Returns(dietician);
            _repositoryMock.Setup(x => x.AddOrUpdate(dietician)).Returns(dietician);

            // act
            Action result = () => _sut.UpdateDietician(updateDietician, dietician.Id);

            // assert
            Assert.Throws<NotFoundException>(result);
        }

        [Fact]
        public void DeleteDietician_DieticianFound_ReturnNull()
        {
            // arrange
            int id = 1;
            Dietician dietician = SampleDietician();
            _repositoryMock.Setup(x => x.GetById(id)).Returns(dietician);
            _repositoryMock.Setup(x => x.Delete(It.IsAny<int>()));
            
            // act
            _sut.DeleteDietician(dietician.Id);

            // assert
            _repositoryMock.Verify(x => x.Delete(dietician.Id), Times.Once);
        }

        [Theory]
        [InlineData(2)]
        [InlineData(3)]
        [InlineData(4)]
        public void DeleteDietician_DieticianNotFound_ReturnNotFoundException(int id)
        {
            // arrange
            Dietician dietician = SampleDietician();
            _repositoryMock.Setup(x => x.GetById(id)).Returns(dietician);
            _repositoryMock.Setup(x => x.Delete(It.IsAny<int>()));

            // act
            Action result = () => _sut.DeleteDietician(dietician.Id);

            // assert
            Assert.Throws<NotFoundException>(result);
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
