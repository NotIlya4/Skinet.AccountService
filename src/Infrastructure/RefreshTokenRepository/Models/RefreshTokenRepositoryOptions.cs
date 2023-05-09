namespace Infrastructure.RefreshTokenSystem.Repository;

public record RefreshTokenRepositoryOptions
{
    public TimeSpan Expire { get; }

    public RefreshTokenRepositoryOptions(TimeSpan expire)
    {
        Expire = expire;
    }
}