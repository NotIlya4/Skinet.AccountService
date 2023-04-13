using Domain.Primitives;

namespace Infrastructure.JwtTokenService;

public interface IJwtTokenService
{
    public Task<JwtTokenPair> AddNewRefreshToken(UserId userId);
    public Task<JwtTokenPair> UpdatePair(UserId userId, RefreshToken refreshToken);
    public Task ExpireRefreshToken(UserId userId, RefreshToken refreshToken);
    public Task ExpireAllRefreshTokens(UserId userId);
    public Task ValidateJwtToken(string jwtToken);
}