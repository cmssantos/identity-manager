using IdentityManager.Domain.ValueObjects;

namespace IdentityManager.Application.Interfaces;

public interface IEmailSender
{
    Task SendEmail(EmailContact sender, EmailContact recipient, string subject, string body);
}
