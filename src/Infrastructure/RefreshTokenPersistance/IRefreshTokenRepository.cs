namespace Infrastructure.RefreshTokenPersistance;

public interface IRefreshTokenRepository
{
    public Task Add(Guid userId, Guid token);
    public Task<Guid> PopAssociatedUser(Guid refreshToken);
}