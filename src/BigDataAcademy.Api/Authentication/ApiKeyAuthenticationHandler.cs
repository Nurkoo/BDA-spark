using System.Security.Claims;
using System.Text.Encodings.Web;
using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Options;

namespace BigDataAcademy.Api.Authentication;

public class ApiKeyAuthenticationHandler : AuthenticationHandler<AuthenticationSchemeOptions>
{
    private const string AuthenticationFailed = "Authentication failed";

    private readonly ApiKeyAuthenticationSettings settings;

    public ApiKeyAuthenticationHandler(
        AppSettings settings,
        IOptionsMonitor<AuthenticationSchemeOptions> options,
        ILoggerFactory logger,
        UrlEncoder encoder,
        ISystemClock clock)
        : base(options, logger, encoder, clock)
    {
        this.settings = settings.Authentication.ApiKey;
    }

    protected override Task<AuthenticateResult> HandleAuthenticateAsync()
    {
        return Task.FromResult(this.HandleAuthenticate());
    }

    private AuthenticateResult HandleAuthenticate()
    {
        var config = this.settings;
        var values = AuthenticationValues.From(this.Request.Headers);

        if (values?.Type != AuthenticationType.ApiKey)
        {
            return AuthenticateResult.NoResult();
        }

        if (string.IsNullOrEmpty(values.Token))
        {
            return AuthenticateResult.Fail(AuthenticationFailed);
        }

        if (values.Token != config.ApiKey)
        {
            return AuthenticateResult.Fail(AuthenticationFailed);
        }

        var claims = new[] { new Claim(ClaimTypes.Authentication, AuthenticationType.ApiKey.Name) };
        var identity = new ClaimsIdentity(claims, AuthenticationType.ApiKey.Name);
        var claimsPrincipal = new ClaimsPrincipal(identity);

        return AuthenticateResult.Success(new AuthenticationTicket(claimsPrincipal, this.Scheme.Name));
    }
}
