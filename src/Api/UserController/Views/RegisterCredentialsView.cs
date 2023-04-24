namespace Api.UserController.Views;

public class RegisterCredentialsView
{
    public string Username { get; }
    public string Email { get; }
    public string Password { get; }

    public RegisterCredentialsView(string username, string email, string password)
    {
        Username = username;
        Email = email;
        Password = password;
    }
}