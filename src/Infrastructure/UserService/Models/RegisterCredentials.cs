using Domain.Primitives;

namespace Infrastructure.UserService.Models;

public class RegisterCredentials
{
    public Name Username { get; }
    public Name Email { get; }
    public Name Password { get; }

    public RegisterCredentials(string username, string email, string password) : this(
        username: new Name(username),
        email: new Name(email),
        password: new Name(password)) { }
    
    public RegisterCredentials(Name username, Name email, Name password)
    {
        Username = username;
        Email = email;
        Password = password;
    }
}