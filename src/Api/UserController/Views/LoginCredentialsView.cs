namespace Api.UserController.Views;

public class LoginCredentialsView
{
    public string Email { get; }
    public string Password { get; }

    public LoginCredentialsView(string email, string password)
    {
        Email = email;
        Password = password;
    }
}