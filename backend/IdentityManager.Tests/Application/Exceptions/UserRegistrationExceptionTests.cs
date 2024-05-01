using IdentityManager.Application.Exceptions;

namespace IdentityManager.Application.Tests.Exceptions;

[CollectionDefinition("UserRegistrationExceptionTests")]
public class UserRegistrationExceptionTests
{
    [Fact]
    public void Constructor_WithMessage_ShouldCreateExceptionWithCorrectMessage()
    {
        // Arrange
        var expectedMessage = "An error occurred during user registration.";

        // Act
        var exception = new UserRegistrationException(expectedMessage);

        // Assert
        Assert.Equal(expectedMessage, exception.Message);
    }

    [Fact]
    public void Constructor_WithMessageAndInnerException_ShouldCreateExceptionWithCorrectMessageAndInnerException()
    {
        // Arrange
        var expectedMessage = "An error occurred during user registration.";
        var innerException = new Exception("Inner exception message");

        // Act
        var exception = new UserRegistrationException(expectedMessage, innerException);

        // Assert
        Assert.Equal(expectedMessage, exception.Message);
        Assert.Equal(innerException, exception.InnerException);
    }
}
