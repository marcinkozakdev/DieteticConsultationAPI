using AutoFixture;
using DieteticConsultationAPI.Entities;
using DieteticConsultationAPI.Exceptions;
using DieteticConsultationAPI.Extensions;
using DieteticConsultationAPI.Models;
using DieteticConsultationAPI.Repositories.Abstractions;
using DieteticConsultationAPI.Services;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
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
            var diet = new DietDto()
            {
                Id = 1,
                Name = "High-protein diet",
                Description = "A diet where 20% or more of your daily calories come from protein. Most high-protein diets are high in saturated fat and severely limit carbohydrate intake.",
                CalorificValue = 1800,
                ProhibitedProducts = "greasy decoctions, margarine, lard, tallow, fast-food, fatty cheeses, blue cheeses, thick groats, pasta, noodles",
                RecommendedProducts = "eggs, fishes, seafood. ,eat. soy products, dairy products",
                Files = new List<FileModelDto>()
            };

            _dietRepositoryMock
                .Setup(x => x.AddOrUpdate(It.IsAny<Diet>()));

            // act
            var _sut = new DietService(_dietRepositoryMock.Object);
            await _sut.Create(diet);

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
                .Setup(x => x.GetAll()).ReturnsAsync(diets);

            // act
            var _sut = new DietService(_dietRepositoryMock.Object);
            var result = await _sut.GetAll();

            // arrange
            _dietRepositoryMock
                .Verify(x => x.GetAll(), Times.Once());

            result.Count().Should().Be(1);
        }

        [Fact]
        public async Task GetAllDiets_DietsListIsEmpty_ReturnNotFoundException()
        {
            // arrange
            List<Diet>? diets = new List<Diet>();


            _dietRepositoryMock
                .Setup(x => x.GetAll()).ReturnsAsync(diets);

            // act
            var _sut = new DietService(_dietRepositoryMock.Object);
            var result = await _sut.GetAll();

            // arrange
            _dietRepositoryMock
                .Verify(x => x.GetAll(), Times.Once());

            result.Count().Should().Be(0);
        }

        [Fact]
        public async Task GetDiet_DietFound_ReturnDiet()
        {
            // arrange
            int id = 1;
            var diet = SampleDiet();

            _dietRepositoryMock
                .Setup(x => x.GetById(id)).ReturnsAsync(diet);

            // act
            var _sut = new DietService(_dietRepositoryMock.Object);
            DietDto result = await _sut.GetById(diet.Id);

            // arrange
            _dietRepositoryMock
                .Verify(x => x.GetById(diet.Id), Times.Once());

            result.Name.Should().NotBe(null);
        }

        [Fact]
        public async Task GetDiet_DietNotFound_ReturnNotFoundException()
        {
            // arrange
            int id = 2;
            var diet = SampleDiet();

            _dietRepositoryMock
                .Setup(x => x.GetById(id)).ReturnsAsync(diet);

            // act
            var _sut = new DietService(_dietRepositoryMock.Object);

            // arrange
            await Assert.ThrowsAsync<CannotFindResourceException>(() => _sut.GetById(diet.Id));
        }

        [Fact]
        public async Task UpdateDiet_DietFound_ReturnDiet()
        {
            // arrange
            int id = 1;
            var diet = SampleDiet();

            var updateDiet = new DietDto()
            {
                Id = 1,
                Name = "High-protein diet",
                Description = "A diet where 20% or more of your daily calories come from protein. Most high-protein diets are high in saturated fat and severely limit carbohydrate intake.",
                CalorificValue = 2200,
                ProhibitedProducts = "greasy decoctions, margarine, lard, tallow, fast-food, fatty cheeses, blue cheeses, thick groats, pasta, noodles",
                RecommendedProducts = "eggs, fishes, seafood. ,eat. soy products, dairy products",
            };

            _dietRepositoryMock
                .Setup(x => x.GetById(id)).ReturnsAsync(diet);

            _dietRepositoryMock
                .Setup(x => x.AddOrUpdate(It.IsAny<Diet>()))
                .ReturnsAsync(diet);

            // act
            var _sut = new DietService(_dietRepositoryMock.Object);
            await _sut.Update(updateDiet);

            // arrange
            _dietRepositoryMock
                .Verify(x => x.AddOrUpdate(It.IsAny<Diet>()), Times.Once());

            Assert.Equal(2200, updateDiet.CalorificValue);
        }

        [Fact]
        public async Task UpdateDiet_DietNotFound_CreateDiet()
        {
            // arrange
            var id = 2;
            var diet = SampleDiet();

            var createDiet = new DietDto()
            {
                Name = "High-protein diet",
                Description = "A diet where 20% or more of your daily calories come from protein. Most high-protein diets are high in saturated fat and severely limit carbohydrate intake.",
                CalorificValue = 2200,
                ProhibitedProducts = "greasy decoctions, margarine, lard, tallow, fast-food, fatty cheeses, blue cheeses, thick groats, pasta, noodles",
                RecommendedProducts = "eggs, fishes, seafood. ,eat. soy products, dairy products",
            };

            _dietRepositoryMock
                .Setup(x => x.GetById(id))
                .ReturnsAsync(diet);

            _dietRepositoryMock
                .Setup(x => x.AddOrUpdate(It.IsAny<Diet>()))
                .ReturnsAsync(diet);

            // act
            var _sut = new DietService(_dietRepositoryMock.Object);
            await _sut.Create(createDiet);

            // arrange
            Assert.Equal("High-protein diet", createDiet.Name);
        }

        [Fact]
        public async Task DeleteDiet_DietFound_ReturnDiet()
        {
            // arrange
            int id = 1;
            var diet = SampleDiet();

            _dietRepositoryMock
                .Setup(x => x.GetById(id))
                .ReturnsAsync(diet);

            _dietRepositoryMock
                .Setup(x => x.Delete(It.IsAny<int>()));

            // act
            var _sut = new DietService(_dietRepositoryMock.Object);
            await _sut.Delete(diet.Id);

            // arrange
            _dietRepositoryMock
                .Verify(x => x.Delete(diet.Id), Times.Once());
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
                Files = new List<FileModel>()
            };
        }
    }
}
