using Domain.Primitives;

namespace Infrastructure.RefreshTokenSystem.Repository;

public interface IRefreshTokenRepository
{
    public Task Add(UserId userId, RefreshToken token);
    public Task StrictDelete(UserId userId, RefreshToken token);
    public Task DeleteAllForUser(UserId userId);
}