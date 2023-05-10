using Domain.Primitives;

namespace Domain.Entities;

public record User
{
    public UserId Id { get; }
    public Username Username { get; }
    public Email Email { get; }

    public User(UserId id, Username username, Email email)
    {
        Id = id;
        Username = username;
        Email = email;
    }
}