namespace BigDataAcademy.Api.Authentication;

public record AuthenticationType
{
    public static readonly AuthenticationType Basic = new(AuthenticationSchemeConstants.Basic);
    public static readonly AuthenticationType ApiKey = new(AuthenticationSchemeConstants.ApiKey);
    public static readonly AuthenticationType OAuth = new(AuthenticationSchemeConstants.OAuth);

    private AuthenticationType(string name)
    {
        this.Name = name;
    }

    public string Name { get; }

    public static AuthenticationType? From(string type)
    {
        return Normalize(type) switch
        {
            AuthenticationSchemeConstants.Basic => Basic,
            AuthenticationSchemeConstants.ApiKey => ApiKey,
            AuthenticationSchemeConstants.OAuth => OAuth,
            _ => null,
        };
    }

    private static string Normalize(string type)
    {
        return type.Trim().ToLower();
    }
}
