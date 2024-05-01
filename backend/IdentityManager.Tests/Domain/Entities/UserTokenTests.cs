using System.Reflection;
using IdentityManager.Domain.Entities;
using IdentityManager.Domain.Types;
using IdentityManager.Domain.ValueObjects;

namespace IdentityManager.Domain.Tests.Entities;

[CollectionDefinition("UserTokenTests")]
public class UserTokenTests
{
    [Fact]
    public void ProtectedConstructor_ShouldInitializeTokenAsEmpty()
    {
        // Use reflection to invoke the protected constructor
        var userTokenType = typeof(UserToken);
        var protectedConstructor = userTokenType.GetConstructor(
            BindingFlags.NonPublic | BindingFlags.Instance,
            null,
            Type.EmptyTypes,
            null);
        Assert.NotNull(protectedConstructor);

        // Create a UserToken instance using the protected constructor
        var userTokenInstance = protectedConstructor.Invoke(null) as UserToken;
        Assert.NotNull(userTokenInstance);
        Assert.Equal(string.Empty, userTokenInstance.Token);
    }

    [Fact]
    public void UserToken_ShouldCreateInstanceForValidArguments()
    {
        // Arrange
        var user = new User("John Doe", new Email("john@example.com"), new Password("V4lidPassword!"));
        var type = TokenType.AccountVerification;
        int expirationInMinutes = 360;

        // Act
        var token = new UserToken(user, type, expirationInMinutes);

        // Assert
        Assert.NotNull(token);
        Assert.Equal(type, token.Type);
        Assert.False(token.IsUsed);
        Assert.False(token.IsRevoked);
        Assert.Equal(expirationInMinutes, (token.ExpirationDate - DateTime.UtcNow).TotalMinutes, precision: 1);
        Assert.NotNull(token.Token);
        Assert.True(token.Token.Length > 0);
    }

    [Fact]
    public void UserToken_ShouldThrowArgumentExceptionForInvalidArguments()
    {
        // Arrange
        var user = new User("John Doe", new Email("john@example.com"), new Password("V4lidPassword!"));

        // Invalid token type
        Assert.Throws<ArgumentException>(() => new UserToken(user, (TokenType)100, 360));

        // Invalid expiration time (non-positive value)
        Assert.Throws<ArgumentException>(() => new UserToken(user, TokenType.Bearer, -1));
    }

    [Fact]
    public void UserToken_MarkAsUsed_ShouldSetIsUsedToTrue()
    {
        // Arrange
        var user = new User("John Doe", new Email("john@example.com"), new Password("V4lidPassword!"));
        var token = new UserToken(user, TokenType.PasswordReset);

        // Act
        token.MarkAsUsed();

        // Assert
        Assert.True(token.IsUsed);

        // Calling MarkAsUsed multiple times should still keep IsUsed as true
        token.MarkAsUsed();
        Assert.True(token.IsUsed);
    }

    [Fact]
    public void UserToken_Revoke_ShouldSetIsRevokedToTrue()
    {
        // Arrange
        var user = new User("John Doe", new Email("john@example.com"), new Password("V4lidPassword!"));
        var token = new UserToken(user, TokenType.Bearer);

        // Act
        token.Revoke();

        // Assert
        Assert.True(token.IsRevoked);

        // Calling Revoke multiple times should still keep IsRevoked as true
        token.Revoke();
        Assert.True(token.IsRevoked);
    }

    [Fact]
    public void UserToken_GenerateSecureRandomString_ShouldReturnUniqueStrings()
    {
        // Arrange
        var user1 = new User("John Doe", new Email("john@example.com"), new Password("V4lidPassword!"));
        var user2 = new User("Jane Doe", new Email("jane@example.com"), new Password("V4lidPassword2!"));

        // Act
        var token1 = new UserToken(user1, TokenType.Bearer);
        var token2 = new UserToken(user2, TokenType.Bearer);

        // Assert
        Assert.NotEqual(token1.Token, token2.Token);
        Assert.NotNull(token1.Token);
        Assert.NotNull(token2.Token);
        Assert.True(token1.Token.Length > 0);
        Assert.True(token2.Token.Length > 0);
    }
}
