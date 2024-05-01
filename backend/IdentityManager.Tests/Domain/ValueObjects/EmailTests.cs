using IdentityManager.Domain.ValueObjects;

namespace IdentityManager.Domain.Tests.ValueObjects;

[CollectionDefinition("EmailTests")]
public class EmailTests
{
    [Fact]
    public void Email_ShouldCreateInstanceForValidEmail()
    {
        // Arrange
        string validEmail = "test@example.com";

        // Act
        var email = new Email(validEmail);

        // Assert
        Assert.NotNull(email);
        Assert.Equal(validEmail, email.Value);
    }

    [Theory]
    [InlineData("")]
    [InlineData(" ")]
    [InlineData("invalid-email")]
    [InlineData("test@")]
    [InlineData("@example.com")]
    [InlineData("test@.example.com")]
    [InlineData("test@@example.com")]
    public void Email_ShouldThrowArgumentExceptionForInvalidEmail(string invalidEmail)
    {
        // Act & Assert
        Assert.Throws<ArgumentException>(() => new Email(invalidEmail));
    }

    [Fact]
    public void Email_Equals_ShouldReturnTrueForEqualEmails()
    {
        // Arrange
        string emailString = "test@example.com";
        var email1 = new Email(emailString);
        var email2 = new Email(emailString);

        // Act & Assert
        Assert.True(email1.Equals(email2));
        Assert.True(email1.Equals((object)email2));
    }

    [Fact]
    public void Email_Equals_ShouldReturnFalseForDifferentEmails()
    {
        // Arrange
        var email1 = new Email("test1@example.com");
        var email2 = new Email("test2@example.com");

        // Act & Assert
        Assert.False(email1.Equals(email2));
    }

    [Fact]
    public void Email_GetHashCode_ShouldReturnSameHashCodeForEqualEmails()
    {
        // Arrange
        string emailString = "test@example.com";
        var email1 = new Email(emailString);
        var email2 = new Email(emailString);

        // Act & Assert
        Assert.Equal(email1.GetHashCode(), email2.GetHashCode());
    }

    [Fact]
    public void Email_GetHashCode_ShouldReturnDifferentHashCodesForDifferentEmails()
    {
        // Arrange
        var email1 = new Email("test1@example.com");
        var email2 = new Email("test2@example.com");

        // Act & Assert
        Assert.NotEqual(email1.GetHashCode(), email2.GetHashCode());
    }

    [Fact]
    public void Email_Equals_ShouldReturnFalseWhenOtherIsNull()
    {
        // Arrange
        var email = new Email("test@example.com");

        // Act
        bool result = email.Equals(null);

        // Assert
        Assert.False(result);
    }

    [Fact]
    public void Email_Equals_ShouldReturnFalseWhenObjIsNotEmail()
    {
        // Arrange
        var email = new Email("test@example.com");
        var notEmailObj = new object();

        // Act
        bool result = email.Equals(notEmailObj);

        // Assert
        Assert.False(result);
    }

    [Fact]
    public void Email_ToString_ShouldReturnEmailValue()
    {
        // Arrange
        string expectedEmail = "test@example.com";
        var email = new Email(expectedEmail);

        // Act
        string emailString = email.ToString();

        // Assert
        Assert.Equal(expectedEmail, emailString);
    }
}
