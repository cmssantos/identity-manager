using IdentityManager.Application.Commands;
using IdentityManager.Application.Commands.Handlers;
using IdentityManager.Application.Exceptions;
using IdentityManager.Application.Interfaces;
using IdentityManager.Application.Notifications;
using IdentityManager.Domain.Entities;
using IdentityManager.Domain.Types;
using MediatR;
using Microsoft.Extensions.Logging;
using Moq;

namespace IdentityManager.Application.Tests.Commands.Handlers;

[CollectionDefinition("RegisterUserCommandHandlerTests")]
public class RegisterUserCommandHandlerTests
{
    private readonly Mock<ILogger<RegisterUserCommandHandler>> _loggerMock;
    private readonly Mock<IUsersRepository> _usersRepositoryMock;
    private readonly Mock<IMediator> _mediatorMock;

    public RegisterUserCommandHandlerTests()
    {
        _loggerMock = new Mock<ILogger<RegisterUserCommandHandler>>();
        _usersRepositoryMock = new Mock<IUsersRepository>();
        _mediatorMock = new Mock<IMediator>();
    }

    [Fact]
    public async Task Handle_ShouldRegisterUserAndPublishNotification_WhenUserDoesNotExist()
    {
        // Arrange
        var request = new RegisterUserCommand
        {
            Username = "test_user",
            Email = "testuser@example.com",
            Password = "Password123!"
        };

        _usersRepositoryMock.Setup(repo => repo.Exists(request.Email)).ReturnsAsync(false);

        var handler = new RegisterUserCommandHandler(
            _loggerMock.Object,
            _usersRepositoryMock.Object,
            _mediatorMock.Object);

        // Act
        await handler.Handle(request, CancellationToken.None);

        // Assert
        // Verifica chamadas para AddAsync, SaveChangesAsync e Publish
        _usersRepositoryMock.Verify(repo => repo.AddAsync(It.IsAny<User>()), Times.Once);
        _usersRepositoryMock.Verify(repo => repo.SaveChangesAsync(), Times.Once);
        _mediatorMock.Verify(mediator => mediator.Publish(
            It.IsAny<UserRegisteredNotification>(), CancellationToken.None), Times.Once);

        // Verifica se LogInformation foi chamado corretamente
        //_loggerMock.Verify(logger => logger.LogInformation(
        //    "User {userId} with email {email} registered successfully.", It.IsAny<Guid>(), request.Email), Times.Once);
    }

    [Fact]
    public async Task Handle_ShouldThrowUserAlreadyExistsException_WhenUserExists()
    {
        // Arrange
        var request = new RegisterUserCommand("test_user", "testuser@example.com", "Password123!");
        _usersRepositoryMock.Setup(repo => repo.Exists(request.Email)).ReturnsAsync(true);

        var handler = new RegisterUserCommandHandler(
            _loggerMock.Object,
            _usersRepositoryMock.Object,
            _mediatorMock.Object);

        // Act & Assert
        var exception = await Assert.ThrowsAsync<UserAlreadyExistsException>(() =>
            handler.Handle(request, CancellationToken.None));

        // Verifica se a mensagem da exceção está correta
        Assert.Equal(ErrorCodes.UserAlreadyExists.ToString(), exception.Message);

        // Verifica se o log de aviso foi registrado corretamente
        //_loggerMock.Verify(logger => logger.LogWarning(
        //    It.IsAny<Exception>(), "Attempt to register user with existing email: {email}", request.Email), Times.Once);
    }

    [Fact]
    public async Task Handle_ShouldThrowArgumentException_WhenInvalidArgumentProvided()
    {
        // Arrange
        var request = new RegisterUserCommand("test_user", "testuser@example.com", "");

        // Configura AddAsync para lançar ArgumentException
        _usersRepositoryMock.Setup(repo => repo.AddAsync(It.IsAny<User>()))
            .ThrowsAsync(new ArgumentException("PasswordCannotBeEmpty"));

        var handler = new RegisterUserCommandHandler(
            _loggerMock.Object,
            _usersRepositoryMock.Object,
            _mediatorMock.Object);

        // Act & Assert
        var exception = await Assert.ThrowsAsync<ArgumentException>(() =>
            handler.Handle(request, CancellationToken.None));

        // Verifica se a mensagem da exceção está correta
        Assert.Equal("PasswordCannotBeEmpty", exception.Message);

        // Verifica se o log de aviso foi registrado corretamente
        //_loggerMock.Verify(logger => logger.LogWarning(
        //    It.IsAny<ArgumentException>(), "Registration attempt failed: invalid argument encountered during registration process for email {email}.", request.Email), Times.Once);
    }

    [Fact]
    public async Task Handle_ShouldThrowUserRegistrationException_WhenUnexpectedExceptionOccurs()
    {
        // Arrange
        var request = new RegisterUserCommand("test_user", "testuser@example.com", "Password123!");

        // Configura SaveChangesAsync para lançar uma exceção inesperada
        _usersRepositoryMock.Setup(repo => repo.SaveChangesAsync())
            .ThrowsAsync(new Exception("Unexpected database error"));

        var handler = new RegisterUserCommandHandler(
            _loggerMock.Object,
            _usersRepositoryMock.Object,
            _mediatorMock.Object);

        // Act & Assert
        var exception = await Assert.ThrowsAsync<UserRegistrationException>(() =>
            handler.Handle(request, CancellationToken.None));

        // Verifica se a mensagem da exceção está correta
        Assert.Equal("An unexpected error occurred during user registration.", exception.Message);

        // Verifica se o log de erro foi registrado corretamente
        //_loggerMock.Verify(logger => logger.LogError(
        //    It.IsAny<Exception>(), "An unexpected error occurred while creating a user with email {email}.", request.Email), Times.Once);
    }
}
