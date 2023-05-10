using Infrastructure.RefreshTokenService.Models;
using Infrastructure.ValidJwtTokenSystem.Models;

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