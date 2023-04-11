namespace Infrastructure.RefreshTokenPersistance;

public record TokenRepositoryOptions
{
    public required TimeSpan JwtRefreshTokenExpireTime { get; init; }
}