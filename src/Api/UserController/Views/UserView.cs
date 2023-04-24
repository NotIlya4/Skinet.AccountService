namespace Api.UserController.Views;

public class UserView
{
    public string Id { get; }
    public string Username { get; }
    public string Email { get; }
    public AddressView? Address { get; }

    public UserView(string id, string username, string email, AddressView? address = null)
    {
        Id = id;
        Username = username;
        Email = email;
        Address = address;
    }
}