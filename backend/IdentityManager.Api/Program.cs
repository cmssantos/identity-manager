using System.Reflection;
using IdentityManager.Api;
using IdentityManager.Application.Configurations;
using IdentityManager.Application.Middlewares;
using IdentityManager.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

// Database Settings
builder.Services.AddDbContext<ApplicationDbContext>(options =>
{
    options.UseNpgsql(connectionString, b =>
    {
        // Configures the migration assembly for IdentityManager.Infrastructure
        b.MigrationsAssembly("IdentityManager.Infrastructure");

        // Defines the schema for the __EFMigrationsHistory table
        b.MigrationsHistoryTable("__EFMigrationsHistory", "identity");
    });
});

// Applications services
builder.Services.AddApplicationServices(builder.Configuration);

// Configure the use of controllers
builder.Services.AddControllers();

builder.Services.AddAuthentication();
builder.Services.AddAuthorization();
builder.Services.AddEndpointsApiExplorer();

builder.Services.AddSwaggerGen(c =>
{
    // Enable XML comments
    var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
    var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
    c.IncludeXmlComments(xmlPath);
});

builder.Services.AddApiVersioning(options =>
{
    // Sets the default API version
    options.DefaultApiVersion = new Microsoft.AspNetCore.Mvc.ApiVersion(1, 0);

    // Allows the customer to specify the desired version
    options.AssumeDefaultVersionWhenUnspecified = true;

    // Defines how the version will be specified in requests
    options.ApiVersionReader = new Microsoft.AspNetCore.Mvc.Versioning.HeaderApiVersionReader("api-version");
    // You can also use QueryStringApiVersionReader, MediaTypeApiVersionReader, or a combination

    // Defines the return policy for calls with incorrect version
    options.ReportApiVersions = true; // Retorna cabeçalho com versões suportadas
});

builder.Configuration.AddEnvironmentVariables();
builder.Services.Configure<EmailSettings>(builder.Configuration.GetSection("EmailSettings"));
builder.Services.Configure<AppSettings>(builder.Configuration.GetSection("AppSettings"));

var app = builder.Build();

// Initializing the database
using (var scope = app.Services.CreateScope())
{
    var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
    context.Database.Migrate();
}

// Add exception handling middleware
app.UseMiddleware<ExceptionHandlingMiddleware>();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// Configure routes
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=home}/{action=Index}/{id?}");

app.UseApiVersioning();

app.UseHttpsRedirection();

// Configure the use of routing, authentication, and authorization
app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();

// Add controller routes
app.MapControllers();

app.Run();
