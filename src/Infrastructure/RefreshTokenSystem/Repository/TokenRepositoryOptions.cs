namespace Infrastructure.RefreshTokenSystem.Repository;

public record TokenRepositoryOptions
{
    public required TimeSpan JwtRefreshTokenExpireTime { get; init; }
}