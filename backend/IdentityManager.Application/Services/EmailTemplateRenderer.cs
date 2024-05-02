using IdentityManager.Application.Interfaces;
using Microsoft.Extensions.Logging;

namespace IdentityManager.Application.Services;

public class EmailTemplateRenderer(ILogger<EmailTemplateRenderer> logger) : IEmailTemplateRenderer
{
    private readonly ILogger<EmailTemplateRenderer> _logger = logger;

    public string RenderTemplate(string template, Dictionary<string, string> data)
    {
        try
        {
            string renderedTemplate = template;
            foreach (var entry in data)
            {
                renderedTemplate = renderedTemplate.Replace($"{{{entry.Key}}}", entry.Value);
            }
            return renderedTemplate;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error rendering email template {template}", template);
            throw;
        }
    }
}
