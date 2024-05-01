using IdentityManager.Domain.Entities;
using IdentityManager.Domain.Types;
using IdentityManager.Domain.ValueObjects;
using IdentityManager.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace IdentityManager.Infrastructure.Tests.Configurations;

[CollectionDefinition("UserTokenConfigurationTests")]
public class UserTokenConfigurationTests
{
    private readonly ApplicationDbContext _context;

    public UserTokenConfigurationTests()
    {
        // Configura o contexto com um banco de dados em memória para isolamento dos testes
        var options = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseInMemoryDatabase($"TestsDb-{Guid.NewGuid()}")
            .Options;
        _context = new ApplicationDbContext(options);
    }

    [Fact]
    public void UserTokenConfiguration_ShouldBeConfiguredCorrectly()
    {
        // Recupera a entidade UserToken
        var entityType = _context.Model.FindEntityType(typeof(UserToken));
        Assert.NotNull(entityType);

        // Verifica a chave primária (Token)
        var primaryKey = entityType.FindPrimaryKey();
        Assert.NotNull(primaryKey);
        Assert.Single(primaryKey.Properties);
        Assert.Equal("Token", primaryKey.Properties[0].Name);

        // Verifica as propriedades
        var tokenProperty = entityType.FindProperty("Token");
        Assert.NotNull(tokenProperty);
        Assert.False(tokenProperty.IsNullable, "Token property should be required.");

        var typeProperty = entityType.FindProperty("Type");
        Assert.NotNull(typeProperty);
        Assert.False(typeProperty.IsNullable, "Type property should be required.");

        var expirationDateProperty = entityType.FindProperty("ExpirationDate");
        Assert.NotNull(expirationDateProperty);
        Assert.False(expirationDateProperty.IsNullable, "ExpirationDate property should be required.");

        var isUsedProperty = entityType.FindProperty("IsUsed");
        Assert.NotNull(isUsedProperty);
        Assert.False(isUsedProperty.IsNullable, "IsUsed property should be required.");

        var isRevokedProperty = entityType.FindProperty("IsRevoked");
        Assert.NotNull(isRevokedProperty);
        Assert.False(isRevokedProperty.IsNullable, "IsRevoked property should be required.");

        // Verifica a relação com User (UserId)
        var userNavigation = entityType.FindNavigation("User");
        Assert.NotNull(userNavigation);
        var foreignKey = userNavigation.ForeignKey;
        Assert.Equal("UserId", foreignKey.Properties[0].Name);
        Assert.Equal(DeleteBehavior.Cascade, foreignKey.DeleteBehavior);

        Dispose();
    }

    [Fact]
    public async Task UserTokenConfiguration_ShouldWorkCorrectlyWithUser()
    {
        // Cria um usuário para o teste
        var user = new User("test_user", new Email("test@example.com"), new Password("Password123!"));
        await _context.Users.AddAsync(user);
        await _context.SaveChangesAsync();

        // Cria um token de usuário
        var userToken = new UserToken(user, TokenType.AccountVerification);

        // Adiciona o token ao contexto
        await _context.UserTokens.AddAsync(userToken);
        await _context.SaveChangesAsync();

        // Recupera o token do banco de dados e verifica as propriedades
        var dbUserToken = await _context.UserTokens.FindAsync(userToken.Token);
        Assert.NotNull(dbUserToken);
        Assert.Equal(userToken.Token, dbUserToken.Token);
        Assert.Equal(userToken.Type, dbUserToken.Type);
        Assert.Equal(userToken.ExpirationDate, dbUserToken.ExpirationDate);
        Assert.Equal(userToken.IsUsed, dbUserToken.IsUsed);
        Assert.Equal(userToken.IsRevoked, dbUserToken.IsRevoked);
        Assert.Equal(user.Id, dbUserToken.UserId);

        Dispose();
    }

    // Limpeza do banco de dados após cada teste
    private void Dispose()
    {
        _context.Database.EnsureDeleted();
        _context.Dispose();
    }
}
