using Domain.Primitives;
using Domain.Primitives.Address;

namespace Domain.Entities;

public record User
{
    public Guid Id { get; }
    public Username Username { get; }
    public Email Email { get; }
    public Address? Address { get; }

    public User(Guid id, Username username, Email email, Address? address = null)
    {
        Id = id;
        Username = username;
        Email = email;
        Address = address;
    }
}