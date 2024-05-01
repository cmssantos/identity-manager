using IdentityManager.Application.Interfaces;

namespace IdentityManager.Application.Services;

public class EmailTemplateRenderer : IEmailTemplateRenderer
{
    public string RenderTemplate(string template, Dictionary<string, string> data)
    {
        string renderedTemplate = template;
        foreach (var entry in data)
        {
            renderedTemplate = renderedTemplate.Replace($"{{{entry.Key}}}", entry.Value);
        }
        return renderedTemplate;
    }
}
