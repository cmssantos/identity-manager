using IdentityManager.Application.Commands;
using IdentityManager.Application.Commands.Handlers;
using IdentityManager.Application.Interfaces;
using IdentityManager.Application.Notifications;
using IdentityManager.Application.Notifications.Handlers;
using IdentityManager.Application.Services;
using IdentityManager.Infrastructure.Repositories;
using IdentityManager.Infrastructure.Services;
using MediatR;

namespace IdentityManager.Api;

public static class ServiceCollectionExtensions
{
    public static void AddApplicationServices(this IServiceCollection services, IConfiguration configuration)
    {
        // Register mediator
        services.AddMediatR(cfg =>
        {
            cfg.RegisterServicesFromAssembly(typeof(Program).Assembly);
            cfg.RegisterServicesFromAssembly(typeof(RegisterUserCommandHandler).Assembly);
        });

        services.AddTransient<IRequestHandler<RegisterUserCommand>, RegisterUserCommandHandler>();
        services.AddTransient<INotificationHandler<UserRegisteredNotification>, UserRegisteredNotificationHandler>();

        // Register repositories
        services.AddScoped<IUsersRepository, UsersRepository>();

        // Register application services
        services.AddScoped<IEmailService, EmailService>();
        services.AddSingleton<ITranslationService, TranslationService>();
        services.AddScoped<IEmailSender, MimeKitEmailSender>();
        services.AddScoped<IEmailTemplateRenderer, EmailTemplateRenderer>();
        services.AddScoped<IEmailTemplateService, EmailTemplateService>();
    }
}
