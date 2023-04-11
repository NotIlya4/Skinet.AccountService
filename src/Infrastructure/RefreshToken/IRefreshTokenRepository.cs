namespace Infrastructure.RefreshTokenPersistance;

public interface IRefreshTokenRepository
{
    public Task Add(UserId userId, Guid token);
    public Task<UserId> PopAssociatedUser(Guid refreshToken);
}