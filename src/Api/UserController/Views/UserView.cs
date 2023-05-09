namespace Api.UserController.Views;

public class UserView
{
    public string Id { get; }
    public string Username { get; }
    public string Email { get; }

    public UserView(string id, string username, string email)
    {
        Id = id;
        Username = username;
        Email = email;
    }
}