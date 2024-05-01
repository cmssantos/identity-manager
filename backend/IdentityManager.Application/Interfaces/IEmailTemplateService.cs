namespace IdentityManager.Application.Interfaces;

public interface IEmailTemplateService
{
    Task<string> LoadTemplateAsync(string templateName);
}
