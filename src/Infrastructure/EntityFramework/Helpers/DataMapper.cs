using Domain.Entities;
using Domain.Primitives;
using Infrastructure.EntityFramework.Models;

namespace Infrastructure.EntityFramework.Helpers;

public class DataMapper
{
    public UserData MapUser(User user, string passwordHash)
    {
        return new UserData(
            id: user.Id.ToString(),
            username: user.Username.Value,
            email: user.Email.Value,
            passwordHash: passwordHash);
    }

    public User MapUser(UserData userData)
    {
        return new User(
            id: new Guid(userData.Id),
            username: new Username(userData.Username),
            email: new Email(userData.Email));
    }
}