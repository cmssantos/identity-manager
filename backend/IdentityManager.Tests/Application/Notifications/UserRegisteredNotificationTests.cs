using IdentityManager.Application.Notifications;

namespace IdentityManager.Application.Tests.Notifications;

[CollectionDefinition("UserRegisteredNotificationTests")]
public class UserRegisteredNotificationTests
{
    [Fact]
    public void DefaultConstructor_ShouldInitializePropertiesWithEmptyStrings()
    {
        // Act
        var notification = new UserRegisteredNotification();

        // Assert
        // Verifica se as propriedades foram inicializadas com strings vazias
        Assert.Equal(string.Empty, notification.Username);
        Assert.Equal(string.Empty, notification.Email);
        Assert.Equal(string.Empty, notification.AccountVerificationToken);
    }

    [Fact]
    public void ParameterizedConstructor_ShouldInitializePropertiesWithGivenValues()
    {
        // Arrange
        var expectedUsername = "test_user";
        var expectedEmail = "testuser@example.com";
        var expectedToken = "verification_token";

        // Act
        var notification = new UserRegisteredNotification(expectedUsername, expectedEmail, expectedToken);

        // Assert
        // Verifica se as propriedades foram inicializadas com os valores fornecidos
        Assert.Equal(expectedUsername, notification.Username);
        Assert.Equal(expectedEmail, notification.Email);
        Assert.Equal(expectedToken, notification.AccountVerificationToken);
    }
}
