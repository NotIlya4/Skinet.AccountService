using Infrastructure.JwtTokenHelper;
using Infrastructure.RefreshTokenRepository;

namespace Infrastructure.JwtTokenPairService;

public class JwtTokenService : IJwtTokenService
{
    private readonly IRefreshTokenService _refreshTokenService;
    private readonly IJwtTokenHelper _jwtTokenHelper;

    public JwtTokenService(IRefreshTokenService refreshTokenService, IJwtTokenHelper jwtTokenHelper)
    {
        _refreshTokenService = refreshTokenService;
        _jwtTokenHelper = jwtTokenHelper;
    }

    public async Task<JwtTokenPair> AddNewRefreshToken(Guid userId)
    {
        Guid newRefreshToken = Guid.NewGuid();
        
        await _refreshTokenService.Add(userId, newRefreshToken);

        string newJwtToken = _jwtTokenHelper.CreateJwtToken(userId);

        return new JwtTokenPair(
            jwtToken: newJwtToken,
            refreshToken: newRefreshToken);
    }

    public async Task<JwtTokenPair> UpdatePair(Guid userId, Guid refreshToken)
    {
        await _refreshTokenService.StrictDelete(userId: userId, token: refreshToken);
        return await AddNewRefreshToken(userId);
    }

    public async Task ExpireRefreshToken(Guid userId, Guid refreshToken)
    {
        await _refreshTokenService.EnsureDeleted(userId, refreshToken);
    }

    public async Task ExpireAllRefreshTokens(Guid userId)
    {
        await _refreshTokenService.DeleteAllForUser(userId);
    }
}