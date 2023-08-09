using System.Reflection;
using BigDataAcademy.Api.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace BigDataAcademy.Api.Swagger;

public static class SwaggerConfigurer
{
    public static void Configure(SwaggerGenOptions options)
    {
        options.AddSecurityDefinition(AuthenticationSchemeConstants.Basic, new OpenApiSecurityScheme()
        {
            Type = SecuritySchemeType.Http,
            Scheme = AuthenticationSchemeConstants.Basic,
            Description = "Input your username and password to access this API",
            In = ParameterLocation.Header,
        });
        options.AddSecurityDefinition(JwtBearerDefaults.AuthenticationScheme, new OpenApiSecurityScheme()
        {
            Type = SecuritySchemeType.Http,
            Scheme = JwtBearerDefaults.AuthenticationScheme,
            Description = "Input bearer token to access this API",
            In = ParameterLocation.Header,
        });
        options.AddSecurityDefinition(AuthenticationSchemeConstants.ApiKey, new OpenApiSecurityScheme()
        {
            Type = SecuritySchemeType.ApiKey,
            Scheme = AuthenticationSchemeConstants.ApiKey,
            Description = "Input apikey to access this API",
            In = ParameterLocation.Header,
            Name = "Authorization",
        });
        options.AddSecurityRequirement(new OpenApiSecurityRequirement
        {
            {
                new OpenApiSecurityScheme
                {
                    Reference = new OpenApiReference { Type = ReferenceType.SecurityScheme, Id = AuthenticationSchemeConstants.ApiKey },
                },
                Array.Empty<string>()
            },
            {
                new OpenApiSecurityScheme
                {
                    Reference = new OpenApiReference { Type = ReferenceType.SecurityScheme, Id = JwtBearerDefaults.AuthenticationScheme },
                },
                Array.Empty<string>()
            },
            {
                new OpenApiSecurityScheme
                {
                    Reference = new OpenApiReference { Type = ReferenceType.SecurityScheme, Id = AuthenticationSchemeConstants.Basic },
                },
                Array.Empty<string>()
            },
        });

        options.OperationFilter<SecurityRequirementsOperationFilter>();
        var xmlFilename = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
        options.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, xmlFilename));
    }
}
