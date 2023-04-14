namespace Api.UserController.Views;

public readonly record struct JwtTokenPairView
{
    public required string JwtToken { get; init; }
    public required string RefreshToken { get; init; }
}