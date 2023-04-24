using Domain.Primitives;

namespace Infrastructure;

public class LoginCredentials
{
    public Name Email { get; }
    public Name Password { get; }

    public LoginCredentials(string email, string password) : this(
        email: new Name(email),
        password: new Name(password)) { }
    
    public LoginCredentials(Name email, Name password)
    {
        Email = email;
        Password = password;
    }
}