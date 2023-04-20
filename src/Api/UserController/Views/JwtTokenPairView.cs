namespace Api.UserController.Views;

public class JwtTokenPairView
{
    public string JwtToken { get; }
    public string RefreshToken { get; }

    public JwtTokenPairView(string jwtToken, string refreshToken)
    {
        JwtToken = jwtToken;
        RefreshToken = refreshToken;
    }
}