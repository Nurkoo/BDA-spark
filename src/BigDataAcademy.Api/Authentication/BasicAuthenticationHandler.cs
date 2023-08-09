using System.Security.Claims;
using System.Text;
using System.Text.Encodings.Web;
using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Options;

namespace BigDataAcademy.Api.Authentication;

public class BasicAuthenticationHandler : AuthenticationHandler<AuthenticationSchemeOptions>
{
    private const string AuthenticationFailed = "Authentication failed";

    private readonly BasicAuthenticationSettings settings;

    public BasicAuthenticationHandler(
        AppSettings settings,
        IOptionsMonitor<AuthenticationSchemeOptions> options,
        ILoggerFactory logger,
        UrlEncoder encoder,
        ISystemClock clock)
        : base(options, logger, encoder, clock)
    {
        this.settings = settings.Authentication.Basic;
    }

    protected override Task<AuthenticateResult> HandleAuthenticateAsync()
    {
        return Task.FromResult(this.HandleAuthenticate());
    }

    private AuthenticateResult HandleAuthenticate()
    {
        var values = AuthenticationValues.From(this.Request.Headers);

        if (values?.Type != AuthenticationType.Basic)
        {
            return AuthenticateResult.NoResult();
        }

        var credentials = Encoding.UTF8.GetString(Convert.FromBase64String(values.Token));

        if (string.IsNullOrEmpty(credentials))
        {
            return AuthenticateResult.Fail(AuthenticationFailed);
        }

        var parts = credentials.Split(":");
        if (parts.Length != 2)
        {
            return AuthenticateResult.Fail(AuthenticationFailed);
        }

        if (parts[0] != this.settings.Username || parts[1] != this.settings.Password)
        {
            return AuthenticateResult.Fail(AuthenticationFailed);
        }

        var claims = new[] { new Claim(ClaimTypes.Authentication, AuthenticationType.Basic.Name) };
        var identity = new ClaimsIdentity(claims, AuthenticationType.Basic.Name);
        var claimsPrincipal = new ClaimsPrincipal(identity);

        return AuthenticateResult.Success(new AuthenticationTicket(claimsPrincipal, this.Scheme.Name));
    }
}
