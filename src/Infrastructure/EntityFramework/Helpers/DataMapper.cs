using Infrastructure.EntityFramework.Models;

namespace Infrastructure.EntityFramework.Helpers;

public class DataMapper
{
    public UserData MapUser(User user, string passwordHash)
    {
        return new UserData(
            id: user.Id.Value.ToString(),
            username: user.Username.Value,
            email: user.Email.Value,
            passwordHash: passwordHash);
    }

    public User MapUser(UserData userData)
    {
        return new User(
            id: new UserId(userData.Id),
            username: new Username(userData.Username),
            email: new Email(userData.Email));
    }
}