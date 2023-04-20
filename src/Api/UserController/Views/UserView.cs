namespace Api.UserController.Views;

public class UserView
{
    public string Id { get; }
    public string Email { get; }
    public AddressView? Address { get; }

    public UserView(string id, string email, AddressView? address = null)
    {
        Id = id;
        Email = email;
        Address = address;
    }
}