namespace Infrastructure.JwtTokenService;

public readonly record struct JwtTokenPair
{
    public required string JwtToken { get; init; }
    public required Guid RefreshToken { get; init; }
}