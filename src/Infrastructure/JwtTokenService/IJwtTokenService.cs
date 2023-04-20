namespace Infrastructure.JwtTokenService;

public interface IJwtTokenService
{
    public Task<JwtTokenPair> AddNewRefreshToken(Guid userId);
    public Task<JwtTokenPair> UpdatePair(Guid userId, Guid refreshToken);
    public Task ExpireRefreshToken(Guid userId, Guid refreshToken);
    public Task ExpireAllRefreshTokens(Guid userId);
}