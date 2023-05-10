namespace Infrastructure.JwtTokenHelper;

public class JwtTokenHelperOptions
{
    public string Secret { get; }
    public TimeSpan Expire { get; }
    public string Issuer { get; }
    public string Audience { get; }

    public JwtTokenHelperOptions(string secret, TimeSpan expire, string issuer, string audience)
    {
        Secret = secret;
        Expire = expire;
        Issuer = issuer;
        Audience = audience;
    }
}