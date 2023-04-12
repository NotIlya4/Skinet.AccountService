using Domain.Primitives;

namespace Infrastructure.JwtTokenService;

public interface IJwtTokenService
{
    public Task<JwtTokenPair> UpdatePair(Guid refreshToken);
    public Task ExpireAllRefreshTokens(UserId userId);
    
}