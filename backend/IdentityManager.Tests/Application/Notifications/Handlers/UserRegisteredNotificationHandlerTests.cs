using IdentityManager.Application.Interfaces;
using IdentityManager.Application.Notifications;
using IdentityManager.Application.Notifications.Handlers;
using IdentityManager.Domain.ValueObjects;
using Moq;

namespace IdentityManager.Application.Tests.Notifications.Handlers;

[CollectionDefinition("UserRegisteredNotificationHandlerTests")]
public class UserRegisteredNotificationHandlerTests
{
    [Fact]
    public async Task Handle_ShouldSendVerificationAccountEmail()
    {
        // Arrange
        var emailServiceMock = new Mock<IEmailService>();
        var handler = new UserRegisteredNotificationHandler(emailServiceMock.Object);

        var notification = new UserRegisteredNotification
        {
            Username = "test_user",
            Email = "testuser@example.com",
            AccountVerificationToken = "verification_token"
        };

        // Act
        await handler.Handle(notification, CancellationToken.None);

        // Assert
        emailServiceMock.Verify(service => service.SendVerificationAccountEmail(
            new EmailContact(notification.Username, new Email(notification.Email)),
            notification.AccountVerificationToken),
            Times.Once);
    }
}
