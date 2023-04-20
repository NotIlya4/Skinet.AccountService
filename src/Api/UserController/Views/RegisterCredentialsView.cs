namespace Api.UserController.Views;

public class RegisterCredentialsView
{
    public string Email { get; }
    public string Password { get; }

    public RegisterCredentialsView(string email, string password)
    {
        Email = email;
        Password = password;
    }
}