namespace BigDataAcademy.Api.Authentication;

public record AuthenticationValues
{
    private AuthenticationValues(AuthenticationType? type, string token)
    {
        this.Type = type;
        this.Token = token;
    }

    public AuthenticationType? Type { get; }

    public string Token { get; }

    public static AuthenticationValues? From(IHeaderDictionary headerDictionary)
    {
        if (!headerDictionary.TryGetValue("Authorization", out var authenticationHeader))
        {
            return null;
        }

        var parts = authenticationHeader
            .ToString()
            .Trim()
            .Split(" ");

        if (parts.Length != 2)
        {
            return null;
        }

        return new AuthenticationValues(AuthenticationType.From(parts[0]), parts[1]);
    }
}
