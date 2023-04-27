namespace Infrastructure.EntityFramework.Models;

public class UserData
{
    public string Id { get; private set; }
    public string Username { get; private set; }
    public string Email { get; private set; }
    public string PasswordHash { get; private set; }
    public string? Country { get; private set; }
    public string? City { get; private set; }
    public string? Street { get; private set; }
    public string? Zipcode { get; private set; }

    public UserData(string id, string username, string email, string passwordHash, string? country, string? city, string? street, string? zipcode)
    {
        Id = id;
        Username = username;
        Email = email;
        PasswordHash = passwordHash;
        Country = country;
        City = city;
        Street = street;
        Zipcode = zipcode;
    }

    private UserData()
    {
        Id = null!;
        Username = null!;
        Email = null!;
        PasswordHash = null!;
    }
}