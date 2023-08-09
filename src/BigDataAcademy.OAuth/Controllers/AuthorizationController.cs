using System.Security.Claims;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;
using OpenIddict.Abstractions;
using OpenIddict.Server.AspNetCore;

namespace BigDataAcademy.OAuth.Controllers;

public class AuthorizationController : Controller
{
    [HttpPost("~/connect/token")]
    [Consumes("application/x-www-form-urlencoded")]
    [EnableRateLimiting("fixed")]
    public SignInResult Exchange([FromForm] OAuthRequest authRequest)
    {
        var request = this.HttpContext.GetOpenIddictServerRequest() ??
                      throw new InvalidOperationException("The OpenID Connect request cannot be retrieved.");

        ClaimsPrincipal claimsPrincipal;

        if (request.IsClientCredentialsGrantType())
        {
            // Note: the client credentials are automatically validated by OpenIddict:
            // if client_id or client_secret are invalid, this action won't be invoked.
            var identity = new ClaimsIdentity(OpenIddictServerAspNetCoreDefaults.AuthenticationScheme);

            // Subject (sub) is a required field, we use the client id as the subject identifier here.
            identity.AddClaim(OpenIddictConstants.Claims.Subject, request.ClientId ?? throw new InvalidOperationException());

            // Add some claim, don't forget to add destination otherwise it won't be added to the access token.
            identity.AddClaim(new Claim(OpenIddictConstants.Claims.Audience, "big-data-academy-api").SetDestinations(OpenIddictConstants.Destinations.AccessToken));

            claimsPrincipal = new ClaimsPrincipal(identity);

            claimsPrincipal.SetScopes(request.GetScopes());
        }
        else
        {
            throw new InvalidOperationException("The specified grant type is not supported.");
        }

        // Returning a SignInResult will ask OpenIddict to issue the appropriate access/identity tokens.
        return this.SignIn(claimsPrincipal, OpenIddictServerAspNetCoreDefaults.AuthenticationScheme);
    }

    public class OAuthRequest
    {
        [FromForm(Name = "client_id")]
        public string? ClientId { get; set; }

        [FromForm(Name = "client_secret")]
        public string? ClientSecret { get; set; }

        [FromForm(Name = "scope")]
        public string? Scope { get; set; }

        [FromForm(Name = "grant_type")]
        public string? GrantType { get; set; }
    }
}
