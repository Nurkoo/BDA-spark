using System.Threading.RateLimiting;
using Microsoft.AspNetCore.RateLimiting;
using Microsoft.EntityFrameworkCore;
using OpenIddict.Abstractions;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services
    .AddDbContext<DbContext>(options => options
        .UseInMemoryDatabase(nameof(DbContext))
        .UseOpenIddict());

builder.Services.AddOpenIddict()
    .AddCore(options => options
        .UseEntityFrameworkCore()
        .UseDbContext<DbContext>())
    .AddServer(options => options
        .SetIssuer(new Uri(builder.Configuration.GetValue<string>("Issuer")!))
        .AllowClientCredentialsFlow()
        .SetTokenEndpointUris("connect/token")
        .SetAccessTokenLifetime(TimeSpan.FromMinutes(5))
        .AddEphemeralEncryptionKey()
        .AddEphemeralSigningKey()
        .DisableAccessTokenEncryption()
        .RegisterScopes("api")
        .UseAspNetCore()
        .DisableTransportSecurityRequirement()
        .EnableTokenEndpointPassthrough());

var fixedPolicy = "fixed";

builder.Services.AddRateLimiter(_ => _
    .AddFixedWindowLimiter(policyName: fixedPolicy, options =>
    {
        options.PermitLimit = 1;
        options.Window = TimeSpan.FromSeconds(60 * 2);
        options.QueueProcessingOrder = QueueProcessingOrder.OldestFirst;
        options.QueueLimit = 0;
    }));

var app = builder.Build();

await using var scope = app.Services.CreateAsyncScope();

var context = scope.ServiceProvider.GetRequiredService<DbContext>();
await context.Database.EnsureCreatedAsync();
var manager = scope.ServiceProvider.GetRequiredService<IOpenIddictApplicationManager>();

if (await manager.FindByClientIdAsync("big-data-academy-api") is null)
{
    await manager.CreateAsync(
        new OpenIddictApplicationDescriptor
        {
            ClientId = "big-data-academy-api",
            ClientSecret = "2f153b74-14e8-4956-bf1e-d9e92d024264-0b00cb2a-b2fd-4f59-9703-57b795822567",
            DisplayName = "big-data-academy-api",
            Permissions =
            {
                OpenIddictConstants.Permissions.Endpoints.Token,
                OpenIddictConstants.Permissions.GrantTypes.ClientCredentials,
                OpenIddictConstants.Permissions.Prefixes.Scope + "api",
            },
        });
}

app.UseSwagger();
app.UseSwaggerUI();
app.UseAuthorization();
app.MapControllers();
app.UseRateLimiter();
app.Run();
