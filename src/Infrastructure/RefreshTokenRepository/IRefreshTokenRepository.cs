namespace Infrastructure.RefreshTokenRepository;

public interface IRefreshTokenRepository
{
    public Task Add(Guid userId, Guid token);
    public Task StrictDelete(Guid userId, Guid token);
    public Task EnsureDeleted(Guid userId, Guid token);
    public Task DeleteAllForUser(Guid userId);
}