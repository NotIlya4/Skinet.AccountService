namespace Infrastructure.RefreshTokenSystem.Repository;

public record RefreshTokenRepositoryOptions
{
    public required TimeSpan Expire { get; init; }
}