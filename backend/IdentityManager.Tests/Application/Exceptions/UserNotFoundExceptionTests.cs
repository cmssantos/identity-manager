using IdentityManager.Application.Exceptions;

namespace IdentityManager.Application.Tests.Exceptions;

[CollectionDefinition("UserNotFoundExceptionTests")]
public class UserNotFoundExceptionTests
{
    [Fact]
    public void Constructor_ShouldCreateExceptionWithCorrectMessage()
    {
        // Arrange
        var expectedMessage = "User not found.";

        // Act
        var exception = new UserNotFoundException(expectedMessage);

        // Assert
        Assert.Equal(expectedMessage, exception.Message);
    }
}
