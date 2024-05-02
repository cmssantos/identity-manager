using IdentityManager.Domain.Entities;
using IdentityManager.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace IdentityManager.Infrastructure.Tests.Configurations;

[CollectionDefinition("UserConfigurationTests")]
public class UserConfigurationTests
{
    private readonly ApplicationDbContext _context;

    public UserConfigurationTests()
    {
        // Configura o contexto com um banco de dados em memória
        var options = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseInMemoryDatabase($"TestsDb-{Guid.NewGuid()}")
            .Options;
        _context = new ApplicationDbContext(options);
    }

    [Fact]
    public void UserConfiguration_ShouldBeConfiguredCorrectly()
    {
        // Recupera a entidade User
        var entityType = _context.Model.FindEntityType(typeof(User));
        Assert.NotNull(entityType);

        // Verifica a chave primária
        var primaryKey = entityType.FindPrimaryKey();
        Assert.NotNull(primaryKey);
        Assert.Single(primaryKey.Properties);
        Assert.Equal("Id", primaryKey.Properties[0].Name);

        // Verifica o índice único em Email
        var emailIndex = entityType.GetIndexes().FirstOrDefault(i => i.Properties.Any(p => p.Name == "Email"));
        Assert.NotNull(emailIndex);
        Assert.True(emailIndex.IsUnique);

        // Verifica as propriedades de Email, Username e Password
        var emailProperty = entityType.FindProperty("Email");
        Assert.NotNull(emailProperty);
        Assert.False(emailProperty.IsNullable);
        Assert.Equal(255, emailProperty.GetMaxLength());

        var usernameProperty = entityType.FindProperty("Username");
        Assert.NotNull(usernameProperty);
        Assert.False(usernameProperty.IsNullable);
        Assert.Equal(50, usernameProperty.GetMaxLength());

        var passwordProperty = entityType.FindProperty("Password");
        Assert.NotNull(passwordProperty);
        Assert.False(passwordProperty.IsNullable);

        // Verifica as propriedades CreatedAt, UpdatedAt e LastLogin
        Assert.NotNull(entityType.FindProperty("CreatedAt"));
        Assert.NotNull(entityType.FindProperty("UpdatedAt"));
        Assert.NotNull(entityType.FindProperty("LastLogin"));

        // Verifica a relação com UserToken
        var navigation = entityType.GetNavigations().FirstOrDefault(n => n.Name == "Tokens");
        Assert.NotNull(navigation);
        var foreignKey = navigation.ForeignKey;
        Assert.Equal("UserId", foreignKey.Properties[0].Name);
        Assert.Equal(DeleteBehavior.Cascade, foreignKey.DeleteBehavior);
    }
}
