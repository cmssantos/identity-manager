using IdentityManager.Application.Exceptions;
using IdentityManager.Application.Interfaces;
using IdentityManager.Application.Notifications;
using IdentityManager.Domain.Entities;
using IdentityManager.Domain.Types;
using IdentityManager.Domain.ValueObjects;
using MediatR;
using Microsoft.Extensions.Logging;

namespace IdentityManager.Application.Commands.Handlers;

public class RegisterUserCommandHandler(
    ILogger<RegisterUserCommandHandler> logger,
    IUsersRepository usersRepository,
    IMediator mediator) : IRequestHandler<RegisterUserCommand>
{
    private readonly ILogger<RegisterUserCommandHandler> _logger = logger;
    private readonly IUsersRepository _usersRepository = usersRepository;
    private readonly IMediator _mediator = mediator;

    public async Task Handle(RegisterUserCommand request, CancellationToken cancellationToken)
    {
        try
        {
            var userExists = await _usersRepository.Exists(request.Email);
            if (userExists)
            {
                throw new UserAlreadyExistsException(ErrorCodes.UserAlreadyExists.ToString());
            }

            var user = new User(request.Username, new Email(request.Email), new Password(request.Password));
            var userToken = new UserToken(user, TokenType.AccountVerification);
            user.AddToken(userToken);

            await _usersRepository.AddAsync(user);
            await _usersRepository.SaveChangesAsync();

            _logger.LogInformation("User {userId} with email {email} registered successfully.", user.Id, request.Email);

            await _mediator.Publish(new UserRegisteredNotification(
                user.Username, user.Email.Value, userToken.Token), cancellationToken);

        }
        catch (UserAlreadyExistsException ex)
        {
            _logger.LogWarning(ex, "Attempt to register user with existing email: {email}", request.Email);
            throw;
        }
        catch (ArgumentException ex)
        {
            _logger.LogWarning(ex, "Registration attempt failed: invalid argument encountered during registration process for email {email}.", request.Email);
            throw;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An unexpected error occurred while creating a user with email {email}.", request.Email);
            throw new UserRegistrationException(ErrorCodes.EmailCannotBeEmpty, "An unexpected error occurred during user registration.");
        }
    }
}
