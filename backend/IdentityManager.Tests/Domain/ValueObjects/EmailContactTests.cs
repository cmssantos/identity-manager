using IdentityManager.Domain.ValueObjects;

namespace IdentityManager.Domain.Tests.ValueObjects;

[CollectionDefinition("EmailContactTests")]
public class EmailContactTests
{
    [Fact]
    public void Constructor_ShouldInitializePropertiesCorrectly()
    {
        // Arrange
        string name = "John Doe";
        var email = new Email("john.doe@example.com");

        // Act
        var emailContact = new EmailContact(name, email);

        // Assert
        Assert.Equal(name, emailContact.Name);
        Assert.Equal(email, emailContact.Email);
    }

    [Fact]
    public void Constructor_ShouldThrowArgumentException_WhenNameIsEmpty()
    {
        // Arrange
        string emptyName = "";
        var email = new Email("john.doe@example.com");

        // Act & Assert
        Assert.Throws<ArgumentException>(() => new EmailContact(emptyName, email));
    }

    [Fact]
    public void Constructor_ShouldThrowArgumentNullException_WhenEmailIsNull()
    {
        // Arrange
        string name = "John Doe";

        // Act & Assert
        Assert.Throws<ArgumentNullException>(() => new EmailContact(name, null!));
    }

    [Fact]
    public void Equals_ShouldReturnTrue_WhenObjectsAreEqual()
    {
        // Arrange
        var email1 = new Email("john.doe@example.com");
        var emailContact1 = new EmailContact("John Doe", email1);

        var email2 = new Email("john.doe@example.com");
        var emailContact2 = new EmailContact("John Doe", email2);

        // Act & Assert
        Assert.True(emailContact1.Equals(emailContact2));
        Assert.True(emailContact1.Equals((object)emailContact2));
    }

    [Fact]
    public void Equals_ShouldReturnFalse_WhenObjectsAreNotEqual()
    {
        // Arrange
        var email1 = new Email("john.doe@example.com");
        var emailContact1 = new EmailContact("John Doe", email1);

        var email2 = new Email("jane.doe@example.com");
        var emailContact2 = new EmailContact("Jane Doe", email2);

        // Act & Assert
        Assert.False(emailContact1.Equals(emailContact2));
    }

    [Fact]
    public void GetHashCode_ShouldReturnSameHashCode_ForEqualObjects()
    {
        // Arrange
        var email1 = new Email("john.doe@example.com");
        var emailContact1 = new EmailContact("John Doe", email1);

        var email2 = new Email("john.doe@example.com");
        var emailContact2 = new EmailContact("John Doe", email2);

        // Act & Assert
        Assert.Equal(emailContact1.GetHashCode(), emailContact2.GetHashCode());
    }

    [Fact]
    public void GetHashCode_ShouldReturnDifferentHashCodes_ForDifferentObjects()
    {
        // Arrange
        var email1 = new Email("john.doe@example.com");
        var emailContact1 = new EmailContact("John Doe", email1);

        var email2 = new Email("jane.doe@example.com");
        var emailContact2 = new EmailContact("Jane Doe", email2);

        // Act & Assert
        Assert.NotEqual(emailContact1.GetHashCode(), emailContact2.GetHashCode());
    }

    [Fact]
    public void ToString_ShouldReturnCorrectStringFormat()
    {
        // Arrange
        var email = new Email("john.doe@example.com");
        var emailContact = new EmailContact("John Doe", email);

        // Act
        var result = emailContact.ToString();

        // Assert
        Assert.Equal("John Doe <john.doe@example.com>", result);
    }
}
