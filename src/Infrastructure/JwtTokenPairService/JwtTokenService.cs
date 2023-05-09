using Infrastructure.JwtTokenManager;
using Infrastructure.RefreshTokenRepository;

namespace Infrastructure.JwtTokenPairService;

public class JwtTokenService : IJwtTokenService
{
    private readonly IRefreshTokenRepository _refreshTokenRepository;
    private readonly IJwtTokenManager _jwtTokenManager;

    public JwtTokenService(IRefreshTokenRepository refreshTokenRepository, IJwtTokenManager jwtTokenManager)
    {
        _refreshTokenRepository = refreshTokenRepository;
        _jwtTokenManager = jwtTokenManager;
    }

    public async Task<JwtTokenPair> AddNewRefreshToken(Guid userId)
    {
        Guid newRefreshToken = Guid.NewGuid();
        
        await _refreshTokenRepository.Add(userId, newRefreshToken);

        string newJwtToken = _jwtTokenManager.CreateJwtToken(userId);

        return new JwtTokenPair(
            jwtToken: newJwtToken,
            refreshToken: newRefreshToken);
    }

    public async Task<JwtTokenPair> UpdatePair(Guid userId, Guid refreshToken)
    {
        await _refreshTokenRepository.StrictDelete(userId: userId, token: refreshToken);
        return await AddNewRefreshToken(userId);
    }

    public async Task ExpireRefreshToken(Guid userId, Guid refreshToken)
    {
        await _refreshTokenRepository.EnsureDeleted(userId, refreshToken);
    }

    public async Task ExpireAllRefreshTokens(Guid userId)
    {
        await _refreshTokenRepository.DeleteAllForUser(userId);
    }
}