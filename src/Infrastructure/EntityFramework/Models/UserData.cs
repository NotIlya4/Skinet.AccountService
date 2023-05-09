namespace Infrastructure.EntityFramework.Models;

public class UserData
{
    public string Id { get; private set; }
    public string Username { get; private set; }
    public string Email { get; private set; }
    public string PasswordHash { get; private set; }

    public UserData(string id, string username, string email, string passwordHash)
    {
        Id = id;
        Username = username;
        Email = email;
        PasswordHash = passwordHash;
    }

    private UserData()
    {
        Id = null!;
        Username = null!;
        Email = null!;
        PasswordHash = null!;
    }
}