using BigDataAcademy.Api.Authentication;
using BigDataAcademy.Api.Authorization;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace BigDataAcademy.Api.Swagger;

public class SecurityRequirementsOperationFilter : IOperationFilter
{
    public void Apply(OpenApiOperation? operation, OperationFilterContext? context)
    {
        if (context != null && operation != null)
        {
            var names1 = context.MethodInfo.DeclaringType?
                .GetCustomAttributes(true)
                .OfType<AuthorizeAttribute>()
                .Select(attr => attr.Policy)
                .Distinct()
                ?? Enumerable.Empty<string>();

            var names2 = context.MethodInfo
                .GetCustomAttributes(true)
                .OfType<AuthorizeAttribute>()
                .Select(attr => attr.Policy)
                .Distinct();

            var policyNames = names1.Concat(names2).ToArray();

            var requireAuth = false;
            var id = string.Empty;

            if (policyNames.Contains(OAuthAuthenticationPolicy.Name))
            {
                requireAuth = true;
                id = JwtBearerDefaults.AuthenticationScheme;
            }
            else if (policyNames.Contains(ApiKeyAuthenticationPolicy.Name))
            {
                requireAuth = true;
                id = AuthenticationSchemeConstants.ApiKey;
            }
            else if (policyNames.Contains(BasicAuthenticationPolicy.Name))
            {
                requireAuth = true;
                id = AuthenticationSchemeConstants.Basic;
            }

            if (requireAuth && !string.IsNullOrEmpty(id))
            {
                operation.Responses.Add("401", new OpenApiResponse { Description = "Unauthorized" });

                operation.Security = new List<OpenApiSecurityRequirement>
                {
                    new()
                    {
                        {
                            new OpenApiSecurityScheme
                            {
                                Reference = new OpenApiReference { Type = ReferenceType.SecurityScheme, Id = id, },
                            },
                            Array.Empty<string>()
                        },
                    },
                };
            }
        }
    }
}
