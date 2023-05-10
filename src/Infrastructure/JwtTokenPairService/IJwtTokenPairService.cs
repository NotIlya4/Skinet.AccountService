using Infrastructure.RefreshTokenService.Models;

namespace Infrastructure.JwtTokenPairService;

public interface IJwtTokenPairService
{
    public Task<JwtTokenPair> CreateNewPair(UserId userId);
    public Task<JwtTokenPair> UpdatePair(UserId userId, RefreshToken refreshToken);
    public Task EnsureRefreshTokenExpired(UserId userId, RefreshToken refreshToken);
    public Task ExpireAllRefreshTokens(UserId userId);
}