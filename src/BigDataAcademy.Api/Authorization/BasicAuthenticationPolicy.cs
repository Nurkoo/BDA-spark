using System.Security.Claims;
using BigDataAcademy.Api.Authentication;
using Microsoft.AspNetCore.Authorization;

namespace BigDataAcademy.Api.Authorization;

public class BasicAuthenticationPolicy : AuthorizationPolicy
{
    public const string Name = nameof(BasicAuthenticationPolicy);

    private static readonly IEnumerable<IAuthorizationRequirement> AuthorizationRequirements = new[]
    {
        new Requirement(),
    };

    private static readonly IEnumerable<string> Schemes = new[]
    {
        AuthenticationSchemeConstants.Basic,
    };

    public BasicAuthenticationPolicy()
        : base(AuthorizationRequirements, Schemes)
    {
    }

    public class Requirement : IAuthorizationRequirement
    {
        public class Handler : AuthorizationHandler<Requirement>
        {
            protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, Requirement requirement)
            {
                var hasClaim = context.User.HasClaim(o => o.Type == ClaimTypes.Authentication && o.Value == AuthenticationType.Basic.Name);

                if (hasClaim)
                {
                    context.Succeed(requirement);
                }

                return Task.CompletedTask;
            }
        }
    }
}
