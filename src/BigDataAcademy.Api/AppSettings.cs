namespace BigDataAcademy.Api;

public class AppSettings
{
    public AuthenticationSettings Authentication { get; set; } = null!;

    public AmazonSettings Aws { get; set; } = null!;
}

public class AuthenticationSettings
{
    public BasicAuthenticationSettings Basic { get; set; } = null!;

    public ApiKeyAuthenticationSettings ApiKey { get; set; } = null!;

    public OAuthAuthenticationSettings OAuth { get; set; } = null!;
}

public class BasicAuthenticationSettings
{
    public string Username { get; set; } = string.Empty;

    public string Password { get; set; } = string.Empty;
}

public class ApiKeyAuthenticationSettings
{
    public string ApiKey { get; set; } = string.Empty;
}

public class OAuthAuthenticationSettings
{
    public string Issuer { get; set; } = string.Empty;
}

public class AmazonSettings
{
    public string S3 { get; set; } = string.Empty;

    public string Kinesis { get; set; } = string.Empty;
}
