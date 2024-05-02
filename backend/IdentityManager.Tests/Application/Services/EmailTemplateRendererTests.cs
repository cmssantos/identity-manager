using IdentityManager.Application.Services;
using Microsoft.Extensions.Logging;
using Moq;

namespace IdentityManager.Application.Tests.Services;

[CollectionDefinition("EmailTemplateRendererTests")]
public class EmailTemplateRendererTests
{
    private readonly Mock<ILogger<EmailTemplateRenderer>> _loggerMock;

    public EmailTemplateRendererTests()
    {
        _loggerMock = new Mock<ILogger<EmailTemplateRenderer>>();
    }

    [Fact]
    public void RenderTemplate_ShouldReplacePlaceholdersWithData()
    {
        // Arrange
        var template = "Hello, {Name}. Your verification code is {Code}.";
        var data = new Dictionary<string, string>
        {
            { "Name", "Alice" },
            { "Code", "12345" }
        };
        var expectedRenderedTemplate = "Hello, Alice. Your verification code is 12345.";

        var renderer = new EmailTemplateRenderer(_loggerMock.Object);

        // Act
        var renderedTemplate = renderer.RenderTemplate(template, data);

        // Assert
        // Checks if markers have been replaced with the correct values
        Assert.Equal(expectedRenderedTemplate, renderedTemplate);
    }

    [Fact]
    public void RenderTemplate_ShouldHandleEmptyData()
    {
        // Arrange
        var template = "Hello, {Name}. Your verification code is {Code}.";
        var data = new Dictionary<string, string>(); // Empty data
        var expectedRenderedTemplate = "Hello, {Name}. Your verification code is {Code}."; // Must remain unchanged

        var renderer = new EmailTemplateRenderer(_loggerMock.Object);

        // Act
        var renderedTemplate = renderer.RenderTemplate(template, data);

        // Assert
        // Checks if the template remains unchanged when the data is empty
        Assert.Equal(expectedRenderedTemplate, renderedTemplate);
    }

    [Fact]
    public void RenderTemplate_ShouldHandleEmptyTemplate()
    {
        // Arrange
        var template = ""; // Empty template
        var data = new Dictionary<string, string>
        {
            { "Name", "Alice" },
            { "Code", "12345" }
        };

        var renderer = new EmailTemplateRenderer(_loggerMock.Object);

        // Act
        var renderedTemplate = renderer.RenderTemplate(template, data);

        // Assert
        Assert.Equal("", renderedTemplate);
    }

    [Fact]
    public void RenderTemplate_ShouldNotReplaceNonExistentPlaceholders()
    {
        // Arrange
        var template = "Hello, {Name}. Your verification code is {Code}. Welcome to {Company}.";
        var data = new Dictionary<string, string>
        {
            { "Name", "Alice" },
            { "Code", "12345" }
        };
        var expectedRenderedTemplate = "Hello, Alice. Your verification code is 12345. Welcome to {Company}.";

        var renderer = new EmailTemplateRenderer(_loggerMock.Object);

        // Act
        var renderedTemplate = renderer.RenderTemplate(template, data);

        // Assert
        // Checks that non-existent placeholders are not replaced
        Assert.Equal(expectedRenderedTemplate, renderedTemplate);
    }
}
