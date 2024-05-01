using IdentityManager.Application.Commands;

namespace IdentityManager.Application.Tests.Commands;

[CollectionDefinition("RegisterUserCommandTests")]
public class RegisterUserCommandTests
{
    [Fact]
    public void DefaultConstructor_ShouldInitializePropertiesWithEmptyStrings()
    {
        // Act
        var command = new RegisterUserCommand();

        // Assert
        Assert.Equal(string.Empty, command.Username);
        Assert.Equal(string.Empty, command.Email);
        Assert.Equal(string.Empty, command.Password);
    }

    [Fact]
    public void ParameterizedConstructor_ShouldInitializePropertiesWithGivenValues()
    {
        // Arrange
        var expectedUsername = "test_user";
        var expectedEmail = "testuser@example.com";
        var expectedPassword = "Password123!";

        // Act
        var command = new RegisterUserCommand(expectedUsername, expectedEmail, expectedPassword);

        // Assert
        Assert.Equal(expectedUsername, command.Username);
        Assert.Equal(expectedEmail, command.Email);
        Assert.Equal(expectedPassword, command.Password);
    }
}
