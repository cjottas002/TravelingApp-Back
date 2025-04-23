using FluentAssertions;
using Moq;
using TravelingApp.Application.Account.Commands.Register;
using TravelingApp.Application.Account.Responses;
using TravelingApp.Application.Account.Responses.Register;
using TravelingApp.Application.Common.Interfaces;

namespace TravelingApp.MSUnitTest.Commands.AuthCommands
{
    [TestClass]
    public class RegisterCommandHandlerTests
    {
        private Mock<IAccountService> _accountServiceMock = null!;
        private RegisterCommandHandler _handler = null!;

        [TestInitialize]
        public void Setup()
        {
            _accountServiceMock = new Mock<IAccountService>();
            _handler = new RegisterCommandHandler(_accountServiceMock.Object);
        }

        [TestMethod]
        public async Task Handle_Should_Return_Success_When_Registration_Succeeds()
        {
            // Arrange
            var command = new RegisterCommand { Username = "newuser", Password = "Admin123!" };

            var successResponse = new FrameworkResponse<RegisterResponse>
            {
                Data = new RegisterResponse { IsRegistered = true },
                Count = 1
            };

            _accountServiceMock
                .Setup(s => s.RegisterAsync(command.Username!, command.Password!))
                .ReturnsAsync(successResponse);

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            result.Success.Should().BeTrue();
            result.Data?.IsRegistered.Should().BeTrue();
        }

        [TestMethod]
        public async Task Handle_Should_Return_Error_When_Username_Already_Exists()
        {
            // Arrange
            var command = new RegisterCommand { Username = "existinguser", Password = "Admin123!" };

            var failedResponse = new FrameworkResponse<RegisterResponse>
            {
                Errors =
                [
                    new("El nombre de usuario ya está registrado", ["Username"])
                ]
            };

            _accountServiceMock
                .Setup(s => s.RegisterAsync(command.Username!, command.Password!))
                .ReturnsAsync(failedResponse);

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            result.Success.Should().BeFalse();
            result.Errors.Should().ContainSingle();
            result.Errors!.First().MemberNames.Should().Contain("Username");
        }

        [TestMethod]
        public async Task Handle_Should_Return_Errors_When_Password_Is_Weak()
        {
            // Arrange
            var command = new RegisterCommand { Username = "newuser", Password = "123" };

            var failedResponse = new FrameworkResponse<RegisterResponse>
            {
                Errors =
                [
                    new("La contraseña es demasiado débil", ["Password"]),
                    new("Debe tener al menos 6 caracteres", ["Password"])
                ]
            };

            _accountServiceMock
                .Setup(s => s.RegisterAsync(command.Username!, command.Password!))
                .ReturnsAsync(failedResponse);

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            result.Success.Should().BeFalse();
            result.Errors.Should().HaveCount(2);
            result.Errors!.All(e => e.MemberNames.Contains("Password")).Should().BeTrue();
        }

        [TestMethod]
        [ExpectedException(typeof(System.Exception))]
        public async Task Handle_Should_Throw_Exception_When_Service_Fails()
        {
            // Arrange
            var command = new RegisterCommand { Username = "erroruser", Password = "Admin123!" };

            _accountServiceMock
                .Setup(s => s.RegisterAsync(command.Username!, command.Password!))
                .ThrowsAsync(new System.Exception("Unexpected error"));

            // Act
            await _handler.Handle(command, CancellationToken.None);
        }
    }
}
