namespace Infrastructure.RefreshToken;

public record TimestampRefreshToken
{
    public required Guid RefreshToken { get; init; }
    public required DateTime Issued { get; init; }
}