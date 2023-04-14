using Domain.Primitives;
using Infrastructure.JwtTokenManager;
using Infrastructure.RefreshTokenSystem.Repository;

namespace Infrastructure.JwtTokenService;

public class JwtTokenService : IJwtTokenService
{
    private readonly IRefreshTokenRepository _refreshTokenRepository;
    private readonly IJwtTokenManager _jwtTokenManager;

    public JwtTokenService(IRefreshTokenRepository refreshTokenRepository, IJwtTokenManager jwtTokenManager)
    {
        _refreshTokenRepository = refreshTokenRepository;
        _jwtTokenManager = jwtTokenManager;
    }

    public async Task<JwtTokenPair> AddNewRefreshToken(UserId userId)
    {
        RefreshToken newRefreshToken = new RefreshToken(Guid.NewGuid());
        
        await _refreshTokenRepository.Add(userId, newRefreshToken);

        string newJwtToken = _jwtTokenManager.CreateJwtToken(userId);

        return new JwtTokenPair()
        {
            JwtToken = newJwtToken,
            RefreshToken = newRefreshToken
        };
    }

    public async Task<JwtTokenPair> UpdatePair(UserId userId, RefreshToken refreshToken)
    {
        await _refreshTokenRepository.StrictDelete(userId, refreshToken);
        return await AddNewRefreshToken(userId);
    }

    public async Task ExpireRefreshToken(UserId userId, RefreshToken refreshToken)
    {
        await _refreshTokenRepository.EnsureDeleted(userId, refreshToken);
    }

    public async Task ExpireAllRefreshTokens(UserId userId)
    {
        await _refreshTokenRepository.DeleteAllForUser(userId);
    }

    public void ValidateJwtToken(string jwtToken)
    {
        _jwtTokenManager.Validate(jwtToken);
    }
}