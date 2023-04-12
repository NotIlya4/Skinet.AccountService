using Domain.Primitives;

namespace Infrastructure.RefreshTokenSystem;

public record TimestampRefreshToken
{
    public required RefreshToken RefreshToken { get; init; }
    public required DateTime Issued { get; init; }
}