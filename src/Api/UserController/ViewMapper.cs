using Api.UserController.Views;
using Domain.Primitives;
using Infrastructure.JwtTokenService;

namespace Api.UserController;

public class ViewMapper
{
    public JwtTokenPairView MapJwtTokenPair(JwtTokenPair jwtTokenPair)
    {
        return new JwtTokenPairView()
        {
            JwtToken = jwtTokenPair.JwtToken,
            RefreshToken = jwtTokenPair.RefreshToken.ToString()
        };
    }

    public JwtTokenPair MapJwtTokenPair(JwtTokenPairView jwtTokenPairView)
    {
        return new JwtTokenPair()
        {
            JwtToken = jwtTokenPairView.JwtToken,
            RefreshToken = new RefreshToken(jwtTokenPairView.RefreshToken)
        };
    }
}