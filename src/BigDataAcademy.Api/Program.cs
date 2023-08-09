using System.Text.Json.Serialization;
using BigDataAcademy.Api;
using BigDataAcademy.Api.Amazon;
using BigDataAcademy.Api.Authentication;
using BigDataAcademy.Api.Authorization;
using BigDataAcademy.Api.Hangfire;
using BigDataAcademy.Api.Initializers;
using BigDataAcademy.Api.Jobs;
using BigDataAcademy.Api.Middlewares;
using BigDataAcademy.Api.Swagger;
using BigDataAcademy.Model;
using FluentValidation;
using Hangfire;
using Hangfire.PostgreSql;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddSingleton<AppSettings>(_ => builder.Configuration.Get<AppSettings>()!);
builder.Services
    .AddAuthentication()
    .AddScheme<AuthenticationSchemeOptions, BasicAuthenticationHandler>(AuthenticationSchemeConstants.Basic, _ => { })
    .AddScheme<AuthenticationSchemeOptions, ApiKeyAuthenticationHandler>(AuthenticationSchemeConstants.ApiKey, _ => { })
    .AddJwtBearer(JwtBearerDefaults.AuthenticationScheme, cfg =>
    {
        cfg.Authority = builder.Configuration.GetValue<string>($"Authentication:{AuthenticationType.OAuth.Name}:Issuer");
        cfg.Audience = "big-data-academy-api";
        cfg.RequireHttpsMetadata = false;
        cfg.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuerSigningKey = false,
        };
    });

builder.Services
    .AddSingleton<IAuthorizationHandler, BasicAuthenticationPolicy.Requirement.Handler>()
    .AddSingleton<IAuthorizationHandler, ApiKeyAuthenticationPolicy.Requirement.Handler>()
    .AddSingleton<IAuthorizationHandler, OAuthAuthenticationPolicy.Requirement.Handler>()
    .AddAuthorization()
    .AddAuthorizationBuilder()
    .AddPolicy(BasicAuthenticationPolicy.Name, new BasicAuthenticationPolicy())
    .AddPolicy(ApiKeyAuthenticationPolicy.Name, new ApiKeyAuthenticationPolicy())
    .AddPolicy(OAuthAuthenticationPolicy.Name, new OAuthAuthenticationPolicy());

builder.Services
    .AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
        options.JsonSerializerOptions.DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull;
    });
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(SwaggerConfigurer.Configure);
builder.Services.AddDbContext<BdaPostgresContext>(options => options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services
    .AddHangfire(configuration => configuration
        .UseSimpleAssemblyNameTypeSerializer()
        .UseRecommendedSerializerSettings()
        .UsePostgreSqlStorage(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddHangfireServer();

builder.Services.AddTransient<IJob, Seeder>();
builder.Services.AddTransient<IJob, EventProcessor>();

builder.Services.AddTransient(AmazonClientFactories.ProvideS3);
builder.Services.AddTransient(AmazonClientFactories.ProvideKinesis);

builder.Services.AddTransient<Initializer>();
builder.Services.AddTransient<IServiceInitializer, KinesisInitializer>();
builder.Services.AddTransient<IServiceInitializer, S3Initializer>();

builder.Services.AddValidatorsFromAssemblyContaining<Program>();

var app = builder.Build();

using var scope = app.Services.CreateScope();
await using var context = scope.ServiceProvider.GetRequiredService<BdaPostgresContext>();
await scope.ServiceProvider.GetRequiredService<Initializer>().Initialize();
context.Database.Migrate();

var appLifetime = scope.ServiceProvider.GetRequiredService<IHostApplicationLifetime>();
var recurringJobManager = scope.ServiceProvider.GetRequiredService<IRecurringJobManager>();
var jobs = scope.ServiceProvider.GetServices<IJob>();
foreach (var job in jobs)
{
    recurringJobManager.AddOrUpdate(job.GetType().Name, () => job.Execute(appLifetime.ApplicationStopping), job.CronExpression, new RecurringJobOptions());
}

if (!context.Events.Any())
{
    recurringJobManager.Trigger(nameof(Seeder));
}

app.UseSwagger();
app.UseSwaggerUI();

app.UseAuthorization();
app.UseHangfireDashboard("/hangfire", new DashboardOptions { Authorization = new[] { new AuthorizationFilter() } });
app.MapHangfireDashboard();
app.MapControllers();
app.UseMiddleware<SwaggerRedirectMiddleware>();
app.UseMiddleware<IntermittentFailureMiddleware>();
app.Run();
