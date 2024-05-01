using IdentityManager.Domain.ValueObjects;

namespace IdentityManager.Domain.Tests.ValueObjects;

[CollectionDefinition("PasswordTests")]
public class PasswordTests
{
    [Fact]
    public void Password_ShouldCreateInstanceForValidPassword()
    {
        // Arrange
        string validPassword = "Test@1234";

        // Act
        var password = new Password(validPassword);

        // Assert
        Assert.NotNull(password);
        Assert.NotNull(password.Value);
    }

    [Theory]
    [InlineData("")]
    [InlineData("short")]
    [InlineData("NOLOWERCASE1@")]
    [InlineData("nouppercase1@")]
    [InlineData("NoDigit!")]
    [InlineData("NoSpecial123")]
    [InlineData("password with spaces")]
    [InlineData("invalid!char")]
    [InlineData("not!allowed")]
    public void Password_ShouldThrowArgumentExceptionForInvalidPassword(string invalidPassword)
    {
        // Act & Assert
        Assert.Throws<ArgumentException>(() => new Password(invalidPassword));
    }

    [Fact]
    public void Password_Validate_ShouldReturnTrueForMatchingPassword()
    {
        // Arrange
        string plainPassword = "Test@1234";
        var password = new Password(plainPassword);

        // Act
        bool isValid = password.Validate(plainPassword);

        // Assert
        Assert.True(isValid);
    }

    [Fact]
    public void Password_Validate_ShouldReturnFalseForNonMatchingPassword()
    {
        // Arrange
        string plainPassword = "Test@1234";
        var password = new Password(plainPassword);

        // Act
        bool isValid = password.Validate("WrongPassword!");

        // Assert
        Assert.False(isValid);
    }

    [Fact]
    public void Password_Equals_ShouldReturnTrueForEqualPasswords()
    {
        // Arrange
        string plainPassword = "Test@1234";
        var password1 = new Password(plainPassword);
        var password2 = new Password(plainPassword);

        // Act & Assert
        Assert.True(password1.Equals(password2));
        Assert.True(password1.Equals((object)password2));
    }

    [Fact]
    public void Password_Equals_ShouldReturnFalseForDifferentPasswords()
    {
        // Arrange
        var password1 = new Password("Test@1234");
        var password2 = new Password("Test@12345");

        // Act & Assert
        Assert.False(password1.Equals(password2));
    }

    [Fact]
    public void Password_GetHashCode_ShouldReturnSameHashCodeForEqualPasswords()
    {
        // Arrange
        string plainPassword = "Test@1234";
        var password1 = new Password(plainPassword);
        var password2 = new Password(plainPassword);

        // Act & Assert
        Assert.Equal(password1.GetHashCode(), password2.GetHashCode());
    }

    [Fact]
    public void Password_GetHashCode_ShouldReturnDifferentHashCodesForDifferentPasswords()
    {
        // Arrange
        var password1 = new Password("Test@1234");
        var password2 = new Password("Test@12345");

        // Act & Assert
        Assert.NotEqual(password1.GetHashCode(), password2.GetHashCode());
    }

    [Fact]
    public void Password_Equals_ShouldReturnFalseWhenOtherIsNull()
    {
        // Arrange
        var password = new Password("Test@1234");

        // Act
        bool result = password.Equals(null);

        // Assert
        Assert.False(result);
    }

    [Fact]
    public void Password_ToString_ShouldReturnHashOfPassword()
    {
        // Arrange
        string plainPassword = "Test@1234";
        var password = new Password(plainPassword);

        // Act
        string passwordString = password.ToString();

        // Assert
        string expectedHash = password.Value; // `Value` stores the password hash
        Assert.Equal(expectedHash, passwordString);
    }

    [Fact]
    public void Password_Equals_ShouldReturnFalseWhenObjIsNotPassword()
    {
        // Arrange
        var password = new Password("Test@1234");
        var notPasswordObj = new object(); // Create an object of different type

        // Act
        bool result = password.Equals(notPasswordObj);

        // Assert
        Assert.False(result);
    }
}
