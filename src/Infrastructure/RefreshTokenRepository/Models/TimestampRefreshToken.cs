namespace Infrastructure.RefreshTokenRepository.Models;

public record TimestampRefreshToken
{
    public Guid RefreshToken { get; }
    public DateTime Issued { get; }

    public TimestampRefreshToken(Guid refreshToken, DateTime issued)
    {
        RefreshToken = refreshToken;
        Issued = issued;
    }
}