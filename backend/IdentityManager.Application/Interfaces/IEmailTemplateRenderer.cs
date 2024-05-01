namespace IdentityManager.Application.Interfaces;

public interface IEmailTemplateRenderer
{
    string RenderTemplate(string template, Dictionary<string, string> data);
}
