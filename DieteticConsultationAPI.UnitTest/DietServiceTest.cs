﻿using AutoFixture;
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
    public class DietServiceTest
    {
        private readonly Mock<IDietRepository> _dietRepositoryMock;
        private readonly Mock<ILogger<DietService>> _loggerMock;

        public DietServiceTest()
        {
            _dietRepositoryMock = new Mock<IDietRepository>();
            _loggerMock = new Mock<ILogger<DietService>>();
        }

        [Fact]
        public async Task CreateDiet_InputDietForTheFoundPatient_ReturnDiet()
        {
            // arrange
            var diet = new CreateDietDto()
            {
                Id = 1,
                Name = "High-protein diet",
                Description = "A diet where 20% or more of your daily calories come from protein. Most high-protein diets are high in saturated fat and severely limit carbohydrate intake.",
                CalorificValue = 1800,
                ProhibitedProducts = "greasy decoctions, margarine, lard, tallow, fast-food, fatty cheeses, blue cheeses, thick groats, pasta, noodles",
                RecommendedProducts = "eggs, fishes, seafood. ,eat. soy products, dairy products",
                PatientId = 2,
            };

            _dietRepositoryMock
                .Setup(x => x.AddOrUpdate(It.IsAny<Diet>()));

            // act
            var _sut = new DietService(_loggerMock.Object, _dietRepositoryMock.Object);
            var result = await _sut.CreateDiet(diet);

            // assert
            _dietRepositoryMock
                .Verify(x => x.AddOrUpdate(It.IsAny<Diet>()), Times.Once());

            Assert.Equal("High-protein diet", diet.Name);
        }

        [Fact]
        public async Task GetAllDiets_DietFound_ReturnAllDiets()
        {
            // arrange
            List<Diet> diets = new List<Diet>()
            {
                SampleDiet()
            };

            _dietRepositoryMock
                .Setup(x => x.GetAllDietsWithPatientsAndFiles()).ReturnsAsync(diets);

            // act
            var _sut = new DietService(_loggerMock.Object, _dietRepositoryMock.Object);
            var result = await _sut.GetAllDiets();

            // arrange
            _dietRepositoryMock
                .Verify(x => x.GetAllDietsWithPatientsAndFiles(), Times.Once());

            result.Count().Should().Be(1);
        }

        [Fact]
        public async Task GetAllDiets_DietsListIsEmpty_ReturnNotFoundException()
        {
            // arrange
            List<Diet>? diets = null;

            _dietRepositoryMock
                .Setup(x => x.GetAllDietsWithPatientsAndFiles()).ReturnsAsync(diets);

            // act
            var _sut = new DietService(_loggerMock.Object, _dietRepositoryMock.Object);

            // arrange
            await Assert.ThrowsAsync<NotFoundHttpException>(() => _sut.GetAllDiets());
        }

        [Fact]
        public async Task GetDiet_DietFound_ReturnDiet()
        {
            // arrange
            int id = 1;
            var diet = SampleDiet();

            _dietRepositoryMock
                .Setup(x => x.GetDietWithPatientAndFiles(id)).ReturnsAsync(diet);

            // act
            var _sut = new DietService(_loggerMock.Object, _dietRepositoryMock.Object);
            DietDto result = await _sut.GetDiet(diet.Id);

            // arrange
            _dietRepositoryMock
                .Verify(x => x.GetDietWithPatientAndFiles(diet.Id), Times.Once());

            result.Name.Should().NotBe(null);
        }

        [Fact]
        public async Task GetDiet_DietNotFound_ReturnNotFoundException()
        {
            // arrange
            int id = 2;
            var diet = SampleDiet();

            _dietRepositoryMock
                .Setup(x => x.GetDietWithPatientAndFiles(id)).ReturnsAsync(diet);

            // act
            var _sut = new DietService(_loggerMock.Object, _dietRepositoryMock.Object);

            // arrange
            await Assert.ThrowsAsync<NotFoundHttpException>(()=> _sut.GetDiet(diet.Id)); 
        }

        [Fact]
        public async Task UpdateDiet_DietFound_ReturnDiet()
        {
            // arrange
            int id = 1;
            var diet = SampleDiet();

            var updateDiet = new UpdateDietDto()
            {
                Name = "High-protein diet",
                Description = "A diet where 20% or more of your daily calories come from protein. Most high-protein diets are high in saturated fat and severely limit carbohydrate intake.",
                CalorificValue = 2200,
                ProhibitedProducts = "greasy decoctions, margarine, lard, tallow, fast-food, fatty cheeses, blue cheeses, thick groats, pasta, noodles",
                RecommendedProducts = "eggs, fishes, seafood. ,eat. soy products, dairy products",
            };

            _dietRepositoryMock
                .Setup(x => x.GetDietWithPatientAndFiles(id)).ReturnsAsync(diet);

            _dietRepositoryMock
                .Setup(x => x.AddOrUpdate(It.IsAny<Diet>()))
                .ReturnsAsync(diet);

            // act
            var _sut = new DietService(_loggerMock.Object, _dietRepositoryMock.Object);
            await _sut.UpdateDiet(updateDiet, diet.Id);

            // arrange
            _dietRepositoryMock
                .Verify(x => x.AddOrUpdate(It.IsAny<Diet>()), Times.Once());

            Assert.Equal(2200, updateDiet.CalorificValue);
        }

        [Fact]
        public async Task UpdateDiet_DietNotFound_ReturnNotFoundException()
        {
            // arrange
            var id = 2;
            var diet = SampleDiet();

            var updateDiet = new UpdateDietDto()
            {
                Name = "High-protein diet",
                Description = "A diet where 20% or more of your daily calories come from protein. Most high-protein diets are high in saturated fat and severely limit carbohydrate intake.",
                CalorificValue = 2200,
                ProhibitedProducts = "greasy decoctions, margarine, lard, tallow, fast-food, fatty cheeses, blue cheeses, thick groats, pasta, noodles",
                RecommendedProducts = "eggs, fishes, seafood. ,eat. soy products, dairy products",
            };

            _dietRepositoryMock
                .Setup(x => x.GetDietWithPatientAndFiles(id))
                .ReturnsAsync(diet);

            _dietRepositoryMock
                .Setup(x => x.AddOrUpdate(It.IsAny<Diet>()))
                .ReturnsAsync(diet);

            // act
            var _sut = new DietService(_loggerMock.Object, _dietRepositoryMock.Object);

            // arrange
            await Assert.ThrowsAsync<NotFoundHttpException>(() => _sut.UpdateDiet(updateDiet, diet.Id));
        }

        [Fact]
        public async Task DeleteDiet_DietFound_ReturnDiet()
        {
            // arrange
            int id = 1;
            var diet = SampleDiet();

            _dietRepositoryMock
                .Setup(x => x.GetDietWithPatientAndFiles(id))
                .ReturnsAsync(diet);

            _dietRepositoryMock
                .Setup(x => x.Delete(It.IsAny<int>()));

            // act
            var _sut = new DietService(_loggerMock.Object, _dietRepositoryMock.Object);
            await _sut.DeleteDiet(diet.Id);

            // arrange
            _dietRepositoryMock
                .Verify(x => x.Delete(diet.Id), Times.Once());
        }

        [Fact]
        public async Task DeleteDiet_DietNotFound_ReturnNotFoundException()
        {
            // arrange
            int id = 2;
            var diet = SampleDiet();

            _dietRepositoryMock
                .Setup(x => x.GetDietWithPatientAndFiles(id))
                .ReturnsAsync(diet);

            _dietRepositoryMock
                .Setup(x => x.Delete(It.IsAny<int>()));

            // act
            var _sut = new DietService(_loggerMock.Object, _dietRepositoryMock.Object);

            // arrange
            await Assert.ThrowsAsync<NotFoundHttpException>(()=> _sut.DeleteDiet(diet.Id)); 
        }

        private Diet SampleDiet()
        {
            return new Diet()
            {
                Id = 1,
                Name = "High-protein diet",
                Description = "A diet where 20% or more of your daily calories come from protein. Most high-protein diets are high in saturated fat and severely limit carbohydrate intake.",
                CalorificValue = 1800,
                ProhibitedProducts = "greasy decoctions, margarine, lard, tallow, fast-food, fatty cheeses, blue cheeses, thick groats, pasta, noodles",
                RecommendedProducts = "eggs, fishes, seafood. ,eat. soy products, dairy products",
            };
        }
    }
}
