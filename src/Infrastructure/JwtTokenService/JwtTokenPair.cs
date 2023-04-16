namespace Infrastructure.JwtTokenService;

public record JwtTokenPair
{
    public string JwtToken { get; }
    public Guid RefreshToken { get; }

    public JwtTokenPair(string jwtToken, Guid refreshToken)
    {
        JwtToken = jwtToken;
        RefreshToken = refreshToken;
    }
}