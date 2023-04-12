namespace Infrastructure.RefreshToken;

public record TokenRepositoryOptions
{
    public required TimeSpan JwtRefreshTokenExpireTime { get; init; }
}