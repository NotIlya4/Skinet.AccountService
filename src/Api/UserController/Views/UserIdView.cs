namespace Api.UserController.Views;

public class UserIdView
{
    public string UserId { get; }

    public UserIdView(string userId)
    {
        UserId = userId;
    }
}