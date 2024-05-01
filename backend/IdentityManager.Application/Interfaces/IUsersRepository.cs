using IdentityManager.Domain.Entities;

namespace IdentityManager.Application.Interfaces;

public interface IUsersRepository
{
    Task<bool> Exists(string email);
    Task AddAsync(User user);
    Task<User?> GetByEmailAsync(string email);
    Task SaveChangesAsync();
}
