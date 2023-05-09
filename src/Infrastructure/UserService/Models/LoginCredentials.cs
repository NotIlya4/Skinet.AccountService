namespace Infrastructure.UserService.Models;

public class LoginCredentials
{
    public string Email { get; }
    public string Password { get; }
    
    public LoginCredentials(string email, string password)
    {
        Email = email;
        Password = password;
    }
}