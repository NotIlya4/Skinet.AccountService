using Infrastructure.JwtTokenHelper;
using Infrastructure.RefreshTokenService.Models;

namespace Infrastructure.JwtTokenPairService;

public record JwtTokenPair
{
    public JwtToken JwtToken { get; }
    public RefreshToken RefreshToken { get; }

    public JwtTokenPair(JwtToken jwtToken, RefreshToken refreshToken)
    {
        JwtToken = jwtToken;
        RefreshToken = refreshToken;
    }
}