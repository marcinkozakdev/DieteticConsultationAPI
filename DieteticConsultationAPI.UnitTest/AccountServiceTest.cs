using DieteticConsultationAPI;
using DieteticConsultationAPI.Entities;
using DieteticConsultationAPI.Exceptions;
using DieteticConsultationAPI.Models;
using DieteticConsultationAPI.Repositories.Abstractions;
using DieteticConsultationAPI.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Moq;
using System.Runtime.CompilerServices;
using Xunit;

namespace DieteticConsultationAPI.UnitTest
{
    public class AccountServiceTest
    {
        private readonly Mock<IAccountRepository> _accountRepositoryMock;
        private readonly Mock<IPasswordHasher<User>> _passwordHasherMock;
        private readonly Mock<AuthenticationSettings> _authenticationSettingsMock;

        public AccountServiceTest()
        {
            _accountRepositoryMock = new Mock<IAccountRepository>();
            _passwordHasherMock = new Mock<IPasswordHasher<User>>();
            _authenticationSettingsMock = new Mock<AuthenticationSettings>();
        }

        [Fact]
        public async Task RegisterUser_CorrectDataProvider_ReturnNewUser()
        {
            // arrange
            var newUser = new RegisterUserDto()
            {
                Email = "marcin.kozak@test.com",
                FirstName = "Marcin",
                LastName = "Kozak",
                RoleId = 1,
                ConfirmPassword = "password1",
                Password = "password1",
            };

            _accountRepositoryMock
                .Setup(x => x.Register(
                    It.IsAny<User>()));

            //act
            var _sut = new AccountService(_passwordHasherMock.Object, _authenticationSettingsMock.Object, _accountRepositoryMock.Object);
            await _sut.RegisterUser(newUser);

            //assert
            _accountRepositoryMock
                .Verify(x => x.Register(It.IsAny<User>()), Times.Once());

            Assert.Equal("marcin.kozak@test.com", newUser.Email);
        }

        [Fact]
        public async Task GenerateJwt_WhenNull_ReturnBadRequestException()
        {
            // arrange
            var loginDto = new LoginDto();

            _accountRepositoryMock
                .Setup(x => x.Login(
                    It.IsAny<LoginDto>()))
                .ReturnsAsync((User?)null);

            //act
            var _sut = new AccountService(_passwordHasherMock.Object, _authenticationSettingsMock.Object, _accountRepositoryMock.Object);

            //assert

            await Assert.ThrowsAsync<BadRequestHttpException>(() => _sut.GenerateJwt(loginDto));
        }

        [Theory]
        [InlineData("test@test.com", "password1")]
        [InlineData("testtest.com", "password123")]
        [InlineData("test@test.com", null)]
        [InlineData(null, "password123")]
        [InlineData(null, null)]
        public async Task GenerateJwt_WhenInvalidUserNameOrPasswordOrNull_ReturnBadRequestException(string email, string password)
        {
            // arrange
            var loginDto = new LoginDto()
            {
                Email = email,
                Password = password,
            };

            _accountRepositoryMock
                .Setup(x => x.Login(
                    It.IsAny<LoginDto>()))
                .ReturnsAsync(new User());

            _passwordHasherMock
                .Setup(x => x.VerifyHashedPassword(
                    It.IsAny<User>(),
                    It.IsAny<string>(),
                    It.IsAny<string>()))
                .Returns(PasswordVerificationResult.Failed);

            //act
            var _sut = new AccountService(_passwordHasherMock.Object, _authenticationSettingsMock.Object, _accountRepositoryMock.Object);

            //assert

            await Assert.ThrowsAsync<BadRequestHttpException>(() => _sut.GenerateJwt(loginDto));
        }
    }
}
