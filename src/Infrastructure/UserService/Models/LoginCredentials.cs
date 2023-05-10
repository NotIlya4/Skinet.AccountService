namespace Infrastructure.UserService.Models;

public class LoginCredentials
{
    public Email Email { get; }
    public Password Password { get; }
    
    public LoginCredentials(Email email, Password password)
    {
        Email = email;
        Password = password;
    }
}