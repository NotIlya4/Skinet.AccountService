namespace Infrastructure.RefreshTokenService.Models;

public record TimestampRefreshToken
{
    public RefreshToken RefreshToken { get; }
    public DateTime Issued { get; }

    public TimestampRefreshToken(RefreshToken refreshToken, DateTime issued)
    {
        RefreshToken = refreshToken;
        Issued = issued;
    }
}