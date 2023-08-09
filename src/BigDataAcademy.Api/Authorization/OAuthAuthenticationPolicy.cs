using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;

namespace BigDataAcademy.Api.Authorization;

public class OAuthAuthenticationPolicy : AuthorizationPolicy
{
    public const string Name = nameof(OAuthAuthenticationPolicy);

    private static readonly IEnumerable<IAuthorizationRequirement> AuthorizationRequirements = new[]
    {
        new Requirement(),
    };

    private static readonly IEnumerable<string> Schemes = new[]
    {
        JwtBearerDefaults.AuthenticationScheme,
    };

    public OAuthAuthenticationPolicy()
        : base(AuthorizationRequirements, Schemes)
    {
    }

    public class Requirement : IAuthorizationRequirement
    {
        public class Handler : AuthorizationHandler<Requirement>
        {
            private readonly OAuthAuthenticationSettings settings;

            public Handler(AppSettings settings)
            {
                this.settings = settings.Authentication.OAuth;
            }

            protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, Requirement requirement)
            {
                var hasClaim = context.User.HasClaim(o => o.Issuer == this.settings.Issuer);
                if (hasClaim)
                {
                    context.Succeed(requirement);
                }

                return Task.CompletedTask;
            }
        }
    }
}
