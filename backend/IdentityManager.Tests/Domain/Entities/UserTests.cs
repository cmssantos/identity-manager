using IdentityManager.Domain.Entities;
using IdentityManager.Domain.ValueObjects;
using IdentityManager.Domain.Types;
using System.Reflection;

namespace IdentityManager.Domain.Tests.Entities;

[CollectionDefinition("UserTests")]
public class UserTests
{
    [Fact]
    public void ProtectedConstructor_ShouldInitializeDefaultValues()
    {
        // Use reflection to access the protected constructor
        var userType = typeof(User);
        var protectedConstructor = userType.GetConstructor(
            BindingFlags.NonPublic | BindingFlags.Instance,
            null,
            Type.EmptyTypes,
            null);
        Assert.NotNull(protectedConstructor);

        // Invoke the protected constructor to create an instance of User
        var userInstance = protectedConstructor.Invoke(null) as User;
        Assert.NotNull(userInstance);
        Assert.NotEqual(Guid.Empty, userInstance.Id);
        Assert.Equal(string.Empty, userInstance.Username);
    }

    [Fact]
    public void User_Creation_ShouldCreateInstanceForValidArguments()
    {
        // Arrange
        string username = "test user";
        var email = new Email("test@example.com");
        var password = new Password("Test@1234");

        // Act
        var user = new User(username, email, password);

        // Assert
        Assert.NotNull(user);
        Assert.Equal(username, user.Username);
        Assert.Equal(email, user.Email);
        Assert.Equal(password, user.Password);
        Assert.False(user.IsActive);
        Assert.NotEqual(default(DateTime), user.CreatedAt);
        Assert.Equal(user.CreatedAt, user.UpdatedAt);
    }

    [Fact]
    public void User_AddToken_ShouldAddTokenAndRevokeExisting_WhenRevokeIsTrue()
    {
        // Arrange
        var user = new User("test user", new Email("test@example.com"), new Password("Test@1234"));
        var token1 = new UserToken(user, TokenType.AccountVerification);
        var token2 = new UserToken(user, TokenType.AccountVerification);

        // Act
        user.AddToken(token1);
        user.AddToken(token2);

        // Assert
        // Checks if tokens were added correctly
        Assert.Equal(2, user.Tokens.Count);
        Assert.Contains(user.Tokens, t => t == token2);
        // Checks if the existing token has been revoked
        Assert.True(token1.IsRevoked);
        // Checks that the new token has not been revoked
        Assert.False(token2.IsRevoked);
    }

    [Fact]
    public void User_AddToken_ShouldThrowException_WhenExistingTokenFoundAndRevokeIsFalse()
    {
        // Arrange
        var user = new User("test user", new Email("test@example.com"), new Password("Test@1234"));
        var token1 = new UserToken(user, TokenType.AccountVerification);
        var token2 = new UserToken(user, TokenType.AccountVerification);

        user.AddToken(token1);

        // Act & Assert
        // Checks if an exception is thrown when an existing token is encountered and revokeExistingToken is false
        Assert.Throws<ArgumentException>(() => user.AddToken(token2, false));
    }

    [Fact]
    public void User_Activate_ShouldSetIsActiveToTrue()
    {
        // Arrange
        var user = new User("test user", new Email("test@example.com"), new Password("Test@1234"));

        // Act
        user.Activate();

        // Assert
        Assert.True(user.IsActive);
        Assert.Equal(DateTime.UtcNow.Date, user.UpdatedAt.Date);
    }

    [Fact]
    public void User_UpdateLastLogin_ShouldSetLastLogin()
    {
        // Arrange
        var user = new User("test user", new Email("test@example.com"), new Password("Test@1234"));

        // Act
        user.UpdateLastLogin();

        // Assert
        Assert.NotNull(user.LastLogin);
        Assert.Equal(DateTime.UtcNow.Date, user.LastLogin.Value.Date);

        // Compare only up to seconds to avoid difference in accuracy
        Assert.Equal(user.LastLogin.Value.TruncateToSeconds(), user.UpdatedAt.TruncateToSeconds());
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData(" ")]
    public void User_Creation_ShouldThrowArgumentExceptionForInvalidUsername(string invalidUsername)
    {
        // Arrange
        var email = new Email("test@example.com");
        var password = new Password("Test@1234");

        // Act & Assert
        Assert.Throws<ArgumentException>(() => new User(invalidUsername, email, password));
    }
}

public static class DateTimeExtensions
{
    // Extension to truncate DateTime to seconds with specific DateTimeKind
    public static DateTime TruncateToSeconds(this DateTime dateTime)
        // Creates a new DateTime with the same year, month, day, hour, minute and second
        // and with the DateTimeKind of the original DateTime
        => new(
            dateTime.Year,
            dateTime.Month,
            dateTime.Day,
            dateTime.Hour,
            dateTime.Minute,
            dateTime.Second,
            dateTime.Kind);
}
