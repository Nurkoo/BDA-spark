using System.Security.Claims;
using BigDataAcademy.Api.Authentication;
using Microsoft.AspNetCore.Authorization;

namespace BigDataAcademy.Api.Authorization;

public class ApiKeyAuthenticationPolicy : AuthorizationPolicy
{
    public const string Name = nameof(ApiKeyAuthenticationPolicy);

    private static readonly IEnumerable<IAuthorizationRequirement> AuthorizationRequirements = new[]
    {
        new Requirement(),
    };

    private static readonly IEnumerable<string> Schemes = new[]
    {
        AuthenticationSchemeConstants.ApiKey,
    };

    public ApiKeyAuthenticationPolicy()
        : base(AuthorizationRequirements, Schemes)
    {
    }

    public class Requirement : IAuthorizationRequirement
    {
        private static readonly AuthenticationType Type = AuthenticationType.ApiKey;

        public class Handler : AuthorizationHandler<Requirement>
        {
            protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, Requirement requirement)
            {
                var hasClaim = context.User.HasClaim(o => o.Type == ClaimTypes.Authentication && o.Value == Type.Name);

                if (hasClaim)
                {
                    context.Succeed(requirement);
                }

                return Task.CompletedTask;
            }
        }
    }
}
