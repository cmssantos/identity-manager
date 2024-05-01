using IdentityManager.Application.Interfaces;
using IdentityManager.Domain.Entities;
using IdentityManager.Domain.ValueObjects;
using IdentityManager.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace IdentityManager.Infrastructure.Repositories;

public class UsersRepository(ApplicationDbContext context) : IUsersRepository
{
    private readonly ApplicationDbContext _context = context;

    public Task<bool> Exists(string email)
        => _context.Users.AnyAsync(u => u.Email == new Email(email));

    public async Task AddAsync(User user) => await _context.Users.AddAsync(user);

    public async Task<User?> GetByEmailAsync(string email)
        => await _context.Users.FirstOrDefaultAsync(u => u.Email == new Email(email));

    public async Task SaveChangesAsync() => await _context.SaveChangesAsync();
}
