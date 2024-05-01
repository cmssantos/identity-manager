using IdentityManager.Application.Interfaces;
using IdentityManager.Domain.Entities;
using IdentityManager.Domain.ValueObjects;
using IdentityManager.Infrastructure.Data;
using IdentityManager.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;

namespace IdentityManager.Infrastructure.Tests.Repositories;

[CollectionDefinition("UsersRepositoryTests")]
public class UsersRepositoryTests
{
    private readonly ApplicationDbContext _context;
    private readonly IUsersRepository _usersRepository;

    public UsersRepositoryTests()
    {
        var options = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseInMemoryDatabase($"TestsDb-{Guid.NewGuid()}")
            .Options;
        _context = new ApplicationDbContext(options);
        _usersRepository = new UsersRepository(_context);
    }

    [Fact]
    public async Task Exists_ShouldReturnTrueWhenEmailExists()
    {
        // Arrange
        var email = new Email("test@example.com");
        var user = new User("test_user", email, new Password("Test@1234"));
        await _context.Users.AddAsync(user);
        await _context.SaveChangesAsync();

        // Act
        var exists = await _usersRepository.Exists(email.Value);

        // Assert
        Assert.True(exists);
        Dispose();
    }

    [Fact]
    public async Task Exists_ShouldReturnFalseWhenEmailDoesNotExist()
    {
        // Arrange
        var email = new Email("test@example.com");

        // Act
        var exists = await _usersRepository.Exists(email.Value);

        // Assert
        Assert.False(exists);
        Dispose();
    }

    [Fact]
    public async Task AddAsync_ShouldAddUserToDatabase()
    {
        // Arrange
        var user = new User("test_user", new Email("test@example.com"), new Password("Test@1234"));

        // Act
        await _usersRepository.AddAsync(user);
        await _usersRepository.SaveChangesAsync();

        // Assert
        var dbUser = await _context.Users.FirstOrDefaultAsync(u => u.Id == user.Id);
        Assert.NotNull(dbUser);
        Dispose();
    }

    [Fact]
    public async Task GetByEmailAsync_ShouldReturnUserWhenEmailExists()
    {
        // Arrange
        var email = new Email("test@example.com");
        var user = new User("test_user", email, new Password("Test@1234"));
        await _context.Users.AddAsync(user);
        await _context.SaveChangesAsync();

        // Act
        var dbUser = await _usersRepository.GetByEmailAsync(email.Value);

        // Assert
        Assert.NotNull(dbUser);
        Assert.Equal(email, dbUser.Email);
        Dispose();
    }

    [Fact]
    public async Task GetByEmailAsync_ShouldReturnNullWhenEmailDoesNotExist()
    {
        // Arrange
        var email = new Email("nonexistent@example.com");

        // Act
        var dbUser = await _usersRepository.GetByEmailAsync(email.Value);

        // Assert
        Assert.Null(dbUser);
        Dispose();
    }

    // Cleaning up the in-memory database after each test
    private void Dispose()
    {
        _context.Database.EnsureDeleted();
    }
}
