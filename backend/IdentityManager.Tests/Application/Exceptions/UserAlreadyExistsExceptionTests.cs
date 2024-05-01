using IdentityManager.Application.Exceptions;

namespace IdentityManager.Application.Tests.Exceptions;

[CollectionDefinition("UserAlreadyExistsExceptionTests")]
public class UserAlreadyExistsExceptionTests
{
    [Fact]
    public void Constructor_ShouldCreateExceptionWithCorrectMessage()
    {
        // Arrange
        var expectedMessage = "User already exists.";

        // Act
        var exception = new UserAlreadyExistsException(expectedMessage);

        // Assert
        Assert.Equal(expectedMessage, exception.Message);
    }
}
