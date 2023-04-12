using Domain.Primitives;

namespace Infrastructure.RefreshToken;

public interface IRefreshTokenRepository
{
    public Task Add(UserId userId, Guid token);
    public Task StrictDelete(UserId userId, Guid token);
    public Task DeleteAllForUser(UserId userId);
}