namespace Api.UserController.Views;

public class JwtTokenPairView
{
    public required string JwtToken { get; init; }
    public required string RefreshToken { get; init; }
}