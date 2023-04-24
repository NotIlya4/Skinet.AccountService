namespace Infrastructure.EntityFramework.Models;

public class UserData
{
    public string Id { get; set; }
    public string Username { get; set; }
    public string Email { get; set; }
    public string PasswordHash { get; set; }
    public string? Country { get; set; }
    public string? City { get; set; }
    public string? Street { get; set; }
    public string? Zipcode { get; set; }

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
}