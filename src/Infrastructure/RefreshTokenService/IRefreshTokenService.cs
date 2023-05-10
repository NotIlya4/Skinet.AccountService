using Infrastructure.RefreshTokenService.Models;

namespace Infrastructure.RefreshTokenService;

public interface IRefreshTokenService
{
    public Task<RefreshToken> CreateNew(UserId userId);
    public Task StrictDelete(UserId userId, RefreshToken token);
    public Task EnsureDeleted(UserId userId, RefreshToken token);
    public Task DeleteAllForUser(UserId userId);
    public Task<bool> Contains(UserId userId, RefreshToken token);
}