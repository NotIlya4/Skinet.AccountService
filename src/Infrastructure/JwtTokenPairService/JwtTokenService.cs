using Infrastructure.JwtTokenHelper;
using Infrastructure.RefreshTokenRepository;

namespace Infrastructure.JwtTokenPairService;

public class JwtTokenService : IJwtTokenService
{
    private readonly IRefreshTokenRepository _refreshTokenRepository;
    private readonly IJwtTokenHelper _jwtTokenHelper;

    public JwtTokenService(IRefreshTokenRepository refreshTokenRepository, IJwtTokenHelper jwtTokenHelper)
    {
        _refreshTokenRepository = refreshTokenRepository;
        _jwtTokenHelper = jwtTokenHelper;
    }

    public async Task<JwtTokenPair> AddNewRefreshToken(Guid userId)
    {
        Guid newRefreshToken = Guid.NewGuid();
        
        await _refreshTokenRepository.Add(userId, newRefreshToken);

        string newJwtToken = _jwtTokenHelper.CreateJwtToken(userId);

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