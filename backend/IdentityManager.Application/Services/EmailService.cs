using IdentityManager.Application.Configurations;
using IdentityManager.Application.Interfaces;
using IdentityManager.Domain.ValueObjects;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace IdentityManager.Application.Services;

public class EmailService(
    IEmailTemplateRenderer emailTemplateRenderer,
    IEmailTemplateService emailTemplateService,
    IOptions<EmailSettings> emailSettings,
    IOptions<AppSettings> appSettings,
    ILogger<EmailService> logger,
    IEmailSender emailSender) : IEmailService
{
    private readonly IEmailTemplateRenderer _emailTemplateRenderer = emailTemplateRenderer;
    private readonly IEmailTemplateService _emailTemplateService = emailTemplateService;
    private readonly EmailSettings _emailSettings = emailSettings.Value;
    private readonly AppSettings _appSettings = appSettings.Value;
    private readonly IEmailSender _emailSender = emailSender;
    private readonly ILogger<EmailService> _logger = logger;

    public async Task SendVerificationAccountEmail(EmailContact recipient, string token)
    {
        string template = await _emailTemplateService.LoadTemplateAsync("RegisteredUserActivation");

        // Create a data dictionary to replace placeholders
        var data = new Dictionary<string, string>
        {
            { "user.Name", recipient.Name },
            { "activationLink", $"{_appSettings.SiteHostAddress}/activate?token={token}" }
        };

        string body = _emailTemplateRenderer.RenderTemplate(template, data);
        string subject = "Activate Your Account";
        var sender = new EmailContact(_emailSettings.SmtpSenderName, new Email(_emailSettings.SmtpSenderEmail));

        _logger.LogInformation("Sending email to {email} with subject {subject}", recipient.Email.Value, subject);
        await _emailSender.SendEmail(sender, recipient, subject, body);
    }
}
