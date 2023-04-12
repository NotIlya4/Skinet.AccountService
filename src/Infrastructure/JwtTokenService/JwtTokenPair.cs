namespace Infrastructure.JwtTokenService;

public readonly record struct JwtTokenPair
{
    public string JwtToken { get; }
    public Guid RefreshToken { get; }

    public JwtTokenPair(string jwtToken, string refreshToken) : this(jwtToken, new Guid(refreshToken))
    {
        
    }
    
    public JwtTokenPair(string jwtToken, Guid refreshToken)
    {
        JwtToken = jwtToken;
        RefreshToken = refreshToken;
    }
}