using IdentityManager.Application.Interfaces;
using IdentityManager.Domain.ValueObjects;
using MediatR;

namespace IdentityManager.Application.Notifications.Handlers;

public class UserRegisteredNotificationHandler(IEmailService emailService)
    : INotificationHandler<UserRegisteredNotification>
{
    private readonly IEmailService _emailService = emailService;

    public async Task Handle(UserRegisteredNotification notification, CancellationToken cancellationToken)
    {
        await _emailService
            .SendVerificationAccountEmail(
                new EmailContact(notification.Username,
                                 new Email(notification.Email)),
                notification.AccountVerificationToken);
    }
}
