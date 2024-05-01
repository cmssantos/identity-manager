using IdentityManager.Application.Configurations;
using IdentityManager.Application.Services;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Moq;

namespace IdentityManager.Application.Tests.Services;

[CollectionDefinition("EmailTemplateServiceTests")]
public class EmailTemplateServiceTests
{
    private readonly Mock<ILogger<EmailTemplateService>> _loggerMock;
    private readonly Mock<IOptions<AppSettings>> _appSettingsMock;

    public EmailTemplateServiceTests()
    {
        _loggerMock = new Mock<ILogger<EmailTemplateService>>();
        _appSettingsMock = new Mock<IOptions<AppSettings>>();
    }

    [Fact]
    public async Task LoadTemplateAsync_ShouldLoadTemplateWhenFileExists()
    {
        // Arrange
        var templateName = "welcome";
        var tempDir = Path.Combine(Path.GetTempPath(), "IdentityManagerTests");
        Directory.CreateDirectory(tempDir);
        var filePath = Path.Combine(tempDir, $"{templateName}.html");
        var expectedContent = "<html>Welcome!</html>";

        // Configure AppSettings
        var appSettings = new AppSettings
        {
            EmailTemplatesDirectory = tempDir
        };
        _appSettingsMock.Setup(opts => opts.Value).Returns(appSettings);

        // Create the temporary file for the test
        File.WriteAllText(filePath, expectedContent);

        var service = new EmailTemplateService(_loggerMock.Object, _appSettingsMock.Object);

        // Act
        var result = await service.LoadTemplateAsync(templateName);

        // Assert
        // Checks if the file contents were loaded correctly
        Assert.Equal(expectedContent, result);

        // Remove the file and directory after testing
        File.Delete(filePath);
        Directory.Delete(tempDir, recursive: true);
    }

    [Fact]
    public async Task LoadTemplateAsync_ShouldThrowFileNotFoundExceptionWhenFileDoesNotExist()
    {
        // Arrange
        var templateName = "welcome";
        var tempDir = Path.Combine(Path.GetTempPath(), "IdentityManagerTests");
        Directory.CreateDirectory(tempDir);
        var filePath = Path.Combine(tempDir, $"{templateName}.html");

        // Configure AppSettings
        var appSettings = new AppSettings
        {
            EmailTemplatesDirectory = tempDir
        };
        _appSettingsMock.Setup(opts => opts.Value).Returns(appSettings);

        var service = new EmailTemplateService(_loggerMock.Object, _appSettingsMock.Object);

        // Act & Assert
        var exception = await Assert.ThrowsAsync<FileNotFoundException>(() =>
            service.LoadTemplateAsync(templateName));

        Assert.Equal($"Template {templateName} not found. Path: {filePath}.", exception.Message);

        // Verifica se LogError foi chamado corretamente
        //_loggerMock.Verify(logger => logger.LogError(
        //    It.IsAny<string>(), templateName, filePath), Times.Once);

        // Remove directory after testing
        Directory.Delete(tempDir, recursive: true);
    }
}
