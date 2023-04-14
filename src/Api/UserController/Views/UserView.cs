namespace Api.UserController.Views;

public class UserView
{
    public required string Id { get; init; }
    public required string Email { get; init; }
    public string? Country { get; init; }
    public string? City { get; init; }
    public string? Street { get; init; }
    public string? Zipcode { get; init; }
}