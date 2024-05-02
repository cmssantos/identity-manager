using IdentityManager.Application.Configurations;
using IdentityManager.Application.Interfaces;
using IdentityManager.Domain.ValueObjects;
using MailKit.Net.Smtp;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using MimeKit;

namespace IdentityManager.Infrastructure.Services;

public class MimeKitEmailSender(IOptions<EmailSettings> emailSettings, ILogger<MimeKitEmailSender> logger) : IEmailSender
{
    private readonly EmailSettings _emailSettings = emailSettings.Value;
    private readonly ILogger<MimeKitEmailSender> _logger = logger;


    public async Task SendEmail(EmailContact sender, EmailContact recipient, string subject, string body)
    {
        var message = new MimeMessage();
        message.From.Add(new MailboxAddress(sender.Name, sender.Email.Value));
        message.To.Add(new MailboxAddress(recipient.Name, recipient.Email.Value));
        message.Subject = subject;
        message.Body = new TextPart("html") { Text = body };

        using var client = new SmtpClient();
        try
        {
            // Connect to SMTP server and authenticate
            await client.ConnectAsync(_emailSettings.SmtpServer, _emailSettings.SmtpPort, useSsl: false);
            await client.AuthenticateAsync(_emailSettings.SmtpUsername, _emailSettings.SmtpPassword);

            _logger.LogInformation("Sending email to {ToEmail} from {FromEmail} with subject {Subject}", recipient.Email.Value, sender.Email.Value, subject);

            await client.SendAsync(message);
        }
        finally
        {
            // Disconnect from SMTP server
            await client.DisconnectAsync(quit: true);
        }
    }
}
