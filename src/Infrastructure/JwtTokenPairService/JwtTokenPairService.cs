using Infrastructure.JwtTokenHelper;
using Infrastructure.RefreshTokenService;
using Infrastructure.RefreshTokenService.Models;
using Infrastructure.ValidJwtTokenSystem.Models;

namespace Infrastructure.JwtTokenPairService;

public class JwtTokenPairService : IJwtTokenPairService
{
    private readonly IRefreshTokenService _refreshTokenService;
    private readonly IJwtTokenHelper _jwtTokenHelper;

    public JwtTokenPairService(IRefreshTokenService refreshTokenService, IJwtTokenHelper jwtTokenHelper)
    {
        _refreshTokenService = refreshTokenService;
        _jwtTokenHelper = jwtTokenHelper;
    }

    public async Task<JwtTokenPair> CreateNewPair(UserId userId)
    {
        RefreshToken newRefreshToken = await _refreshTokenService.CreateNew(userId);

        JwtToken newJwtToken = _jwtTokenHelper.Create(userId);

        return new JwtTokenPair(
            jwtToken: newJwtToken,
            refreshToken: newRefreshToken);
    }

    public async Task<JwtTokenPair> UpdatePair(UserId userId, RefreshToken refreshToken)
    {
        await _refreshTokenService.StrictDelete(userId, refreshToken);
        return await CreateNewPair(userId);
    }

    public async Task EnsureRefreshTokenExpired(UserId userId, RefreshToken refreshToken)
    {
        await _refreshTokenService.EnsureDeleted(userId, refreshToken);
    }

    public async Task ExpireAllRefreshTokens(UserId userId)
    {
        await _refreshTokenService.DeleteAllForUser(userId);
    }
}