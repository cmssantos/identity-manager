using IdentityManager.Domain.ValueObjects;

namespace IdentityManager.Application.Interfaces;

public interface IEmailService
{
    Task SendVerificationAccountEmail(EmailContact recipient, string token);
}
