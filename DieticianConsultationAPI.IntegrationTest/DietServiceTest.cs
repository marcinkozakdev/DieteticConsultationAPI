using AutoFixture;
using DieteticConsultationAPI.Entities;
using DieteticConsultationAPI.Exceptions;
using DieteticConsultationAPI.Models;
using DieteticConsultationAPI.Repositories.Abstractions;
using DieteticConsultationAPI.Services;
using FluentAssertions;
using Moq;
using Xunit;

namespace DieticianConsultationAPI.UnitTest
{
    public class DietServiceTest
    {
        private readonly Mock<IDietRepository> _repositoryMock;
        private readonly Mock<ILogger<DietService>> _loggerMock;
        private readonly DietService _sut;

        public DietServiceTest()
        {
            _repositoryMock = new Mock<IDietRepository>();
            _loggerMock = new Mock<ILogger<DietService>>();
            _sut = new DietService(_loggerMock.Object, _repositoryMock.Object);
        }

        [Fact]
        public void CreateDiet_InputDietForTheFoundPatient_ReturnDiet()
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
            _repositoryMock.Setup(x => x.AddOrUpdate(It.IsAny<Diet>()));

            // act
            var result = _sut.CreateDiet(diet);

            // assert
            _repositoryMock.Verify(x => x.AddOrUpdate(It.IsAny<Diet>()), Times.Once());
            result.Should().NotBe(null);
            Assert.Equal("High-protein diet", diet.Name);
        }

        [Fact]
        public void GetAllDiets_DietFound_ReturnAllDiets()
        {
            // arrange
            List<Diet> diets = new List<Diet>()
            {
                SampleDiet()
            };
            _repositoryMock.Setup(x => x.GetAll()).Returns(diets);

            // act
            var result = _sut.GetAllDiets();

            // arrange
            _repositoryMock.Verify(x => x.GetAll(), Times.Once());
            result.Count().Should().Be(1);
        }

        [Fact]
        public void GetAllDiets_DietsListIsEmpty_ReturnNotFoundException()
        {
            // arrange
            List<Diet>? diets = null;
            _repositoryMock.Setup(x => x.GetAll()).Returns(diets);

            // act
            Action result = () => _sut.GetAllDiets();

            // arrange
           Assert.Throws<NotFoundException>(result);
        }

        [Fact]
        public void GetDiet_DietFound_ReturnDiet()
        {
            // arrange
            int id = 1;
            Diet diet = SampleDiet();
            _repositoryMock.Setup(x => x.GetById(id)).Returns(diet);

            // act
            DietDto result = _sut.GetDiet(diet.Id);

            // arrange
            _repositoryMock.Verify(x => x.GetById(diet.Id), Times.Once());
            result.Name.Should().NotBe(null);
        }

        [Theory]
        [InlineData(2)]
        [InlineData(3)]
        [InlineData(4)]
        public void GetDiet_DietNotFound_ReturnNotFoundException(int id)
        {
            // arrange
            Diet diet = SampleDiet();
            _repositoryMock.Setup(x => x.GetById(id)).Returns(diet);

            // act
            Action result = () => _sut.GetDiet(diet.Id);

            // arrange
            Assert.Throws<NotFoundException>(result);
        }

        [Fact]
        public void UpdateDiet_DietFound_ReturnDiet()
        {
            // arrange
            int id = 1;
            Diet diet = SampleDiet();
            UpdateDietDto updateDiet = new UpdateDietDto()
            {
                Name = "High-protein diet",
                Description = "A diet where 20% or more of your daily calories come from protein. Most high-protein diets are high in saturated fat and severely limit carbohydrate intake.",
                CalorificValue = 2200,
                ProhibitedProducts = "greasy decoctions, margarine, lard, tallow, fast-food, fatty cheeses, blue cheeses, thick groats, pasta, noodles",
                RecommendedProducts = "eggs, fishes, seafood. ,eat. soy products, dairy products",
            };
            _repositoryMock.Setup(x => x.GetById(id)).Returns(diet);
            _repositoryMock.Setup(x => x.AddOrUpdate(It.IsAny<Diet>())).Returns(diet);

            // act
            _sut.UpdateDiet(updateDiet, diet.Id);

            // arrange
            _repositoryMock.Verify(x => x.AddOrUpdate(It.IsAny<Diet>()), Times.Once());
            Assert.Equal(2200, updateDiet.CalorificValue);
        }

        [Theory]
        [InlineData(2)]
        [InlineData(3)]
        [InlineData(4)]
        public void UpdateDiet_DietNotFound_ReturnNotFoundException(int id)
        {
            // arrange
            Diet diet = SampleDiet();
            UpdateDietDto updateDiet = new UpdateDietDto()
            {
                Name = "High-protein diet",
                Description = "A diet where 20% or more of your daily calories come from protein. Most high-protein diets are high in saturated fat and severely limit carbohydrate intake.",
                CalorificValue = 2200,
                ProhibitedProducts = "greasy decoctions, margarine, lard, tallow, fast-food, fatty cheeses, blue cheeses, thick groats, pasta, noodles",
                RecommendedProducts = "eggs, fishes, seafood. ,eat. soy products, dairy products",
            };
            _repositoryMock.Setup(x => x.GetById(id)).Returns(diet);
            _repositoryMock.Setup(x => x.AddOrUpdate(It.IsAny<Diet>())).Returns(diet);

            // act
            Action result = () => _sut.UpdateDiet(updateDiet, diet.Id);

            // arrange
            Assert.Throws<NotFoundException>(result);
        }

        [Fact]
        public void DeleteDiet_DietFound_ReturnDiet()
        {
            // arrange
            int id = 1;
            Diet diet = SampleDiet();
            _repositoryMock.Setup(x => x.GetById(id)).Returns(diet);
            _repositoryMock.Setup(x => x.Delete(It.IsAny<int>()));

            // act
            _sut.DeleteDiet(diet.Id);

            // arrange
            _repositoryMock.Verify(x => x.Delete(diet.Id), Times.Once());
        }

        [Theory]
        [InlineData(2)]
        [InlineData(3)]
        [InlineData(4)]
        public void DeleteDiet_DietNotFound_ReturnNotFoundException(int id)
        {
            // arrange
            Diet diet = SampleDiet();
            _repositoryMock.Setup(x => x.GetById(id)).Returns(diet);
            _repositoryMock.Setup(x => x.Delete(It.IsAny<int>())).Returns(diet);

            // act
            Action result = () => _sut.DeleteDiet(diet.Id);

            // arrange
            Assert.Throws<NotFoundException>(result);
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
