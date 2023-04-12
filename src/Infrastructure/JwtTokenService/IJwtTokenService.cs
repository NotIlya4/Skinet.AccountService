using Domain.Primitives;

namespace Infrastructure.JwtTokenService;

public interface IJwtTokenService
{
    public Task<JwtTokenPair> UpdatePair(UserId userId, Guid refreshToken);
    public Task ExpireAllRefreshTokens(UserId userId);
    
}