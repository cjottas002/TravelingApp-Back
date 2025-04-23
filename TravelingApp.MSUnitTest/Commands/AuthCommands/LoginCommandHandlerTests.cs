using FluentAssertions;
using Moq;
using System.ComponentModel.DataAnnotations;
using TravelingApp.Application.Account.Commands.Login;
using TravelingApp.Application.Account.Responses;
using TravelingApp.Application.Account.Responses.Login;
using TravelingApp.Application.Common.Interfaces;

namespace TravelingApp.MSUnitTest.Commands.AuthCommands
{
    [TestClass]
    public class LoginCommandHandlerTests
    {
        private Mock<IAccountService> _accountServiceMock = null!;
        private LoginCommandHandler _handler = null!;

        [TestInitialize]
        public void Setup()
        {
            _accountServiceMock = new Mock<IAccountService>();
            _handler = new LoginCommandHandler(_accountServiceMock.Object);
        }

        [TestMethod]
        public async Task Handle_Should_Return_Valid_Response_When_Login_Succeeds()
        {
            // Arrange
            var command = new LoginCommand { Username = "admin@test.com", Password = "admin" };

            var expectedResponse = new FrameworkResponse<LoginResponse>
            {
                Data = new LoginResponse
                {
                    UserId = "123",
                    Token = "token123"
                },
                Count = 1
            };

            _accountServiceMock
                .Setup(s => s.LoginAsync(command.Username!, command.Password!))
                .ReturnsAsync(expectedResponse);

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            result.Should().NotBeNull();
            result.Success.Should().BeTrue();
            result.Data?.UserId.Should().Be("123");
            result.Data?.Token.Should().Be("token123");
        }

        [TestMethod]
        public async Task Handle_Should_Return_Error_When_User_Not_Found()
        {
            // Arrange
            var command = new LoginCommand { Username = "notfound@test.com", Password = "admin" };

            var failedResponse = new FrameworkResponse<LoginResponse>
            {
                Errors = new List<ValidationResult>
                {
                    new("Usuario no encontrado", ["Username"])
                }
            };

            _accountServiceMock
                .Setup(s => s.LoginAsync(command.Username!, command.Password!))
                .ReturnsAsync(failedResponse);

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            result.Success.Should().BeFalse();
            result.Errors.Should().ContainSingle();
            result.Errors!.First().ErrorMessage.Should().Be("Usuario no encontrado");
        }

        [TestMethod]
        public async Task Handle_Should_Return_Error_When_Invalid_Password()
        {
            // Arrange
            var command = new LoginCommand { Username = "admin@test.com", Password = "wrongpass" };

            var failedResponse = new FrameworkResponse<LoginResponse>
            {
                Errors =
                [
                    new("Credenciales inválidas", ["Password"])
                ]
            };

            _accountServiceMock
                .Setup(s => s.LoginAsync(command.Username!, command.Password!))
                .ReturnsAsync(failedResponse);

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            result.Success.Should().BeFalse();
            result.Errors.Should().ContainSingle();
            result.Errors!.First().MemberNames.Should().Contain("Password");
        }

        [TestMethod]
        public async Task Handle_Should_Return_Multiple_Errors()
        {
            // Arrange
            var command = new LoginCommand { Username = "", Password = "" };

            var failedResponse = new FrameworkResponse<LoginResponse>
            {
                Errors =
                [
                    new("Campo obligatorio", ["Username"]),
                    new("Campo obligatorio", ["Password"])
                ]
            };

            _accountServiceMock
                .Setup(s => s.LoginAsync(command.Username!, command.Password!))
                .ReturnsAsync(failedResponse);

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            result.Success.Should().BeFalse();
            result.Errors.Should().HaveCount(2);
        }

        [TestMethod]
        [ExpectedException(typeof(Exception))]
        public async Task Handle_Should_Throw_When_Service_Fails()
        {
            // Arrange
            var command = new LoginCommand { Username = "admin@test.com", Password = "admin" };

            _accountServiceMock
                .Setup(s => s.LoginAsync(command.Username!, command.Password!))
                .ThrowsAsync(new Exception("Unexpected error"));

            // Act
            await _handler.Handle(command, CancellationToken.None);
        }
    }
}
