using IdentityManager.Application.Configurations;
using IdentityManager.Application.Interfaces;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace IdentityManager.Application.Services;

public class EmailTemplateService(
    ILogger<EmailTemplateService> logger,
    IOptions<AppSettings> appSettings) : IEmailTemplateService
{
    private readonly ILogger<EmailTemplateService> _logger = logger;
    private readonly AppSettings _appSettings = appSettings.Value;

    public async Task<string> LoadTemplateAsync(string templateName)
    {
        var filePath = Path.Combine(_appSettings.EmailTemplatesDirectory, $"{templateName}.html");
        if (!File.Exists(filePath))
        {
            _logger.LogError("Template {templateName} not found. Path:  {filePath}.", templateName, filePath);
            throw new FileNotFoundException($"Template {templateName} not found. Path: {filePath}.");
        }
        return await File.ReadAllTextAsync(filePath);
    }
}

