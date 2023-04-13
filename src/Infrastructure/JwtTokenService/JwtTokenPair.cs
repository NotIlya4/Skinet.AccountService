using Domain.Primitives;

namespace Infrastructure.JwtTokenService;

public readonly record struct JwtTokenPair
{
    public required string JwtToken { get; init; }
    public required RefreshToken RefreshToken { get; init; }
}