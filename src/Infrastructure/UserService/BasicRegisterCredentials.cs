namespace Infrastructure.UserService;

public readonly record struct BasicRegisterCredentials
{
    public required string Email { get; init; }
    public required string Password { get; init; }
}