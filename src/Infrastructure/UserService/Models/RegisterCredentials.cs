using Domain.Primitives;

namespace Infrastructure.UserService.Models;

public class RegisterCredentials
{
    public Username Username { get; }
    public Email Email { get; }
    public Password Password { get; }

    public RegisterCredentials(string username, string email, string password) : this(
        username: new Username(username),
        email: new Email(email),
        password: new Password(password)) { }
    
    public RegisterCredentials(Username username, Email email, Password password)
    {
        Username = username;
        Email = email;
        Password = password;
    }
}