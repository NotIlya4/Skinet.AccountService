using Domain.Primitives;

namespace Domain.Entities;

public record User
{
    public Guid Id { get; }
    public Name Username { get; }
    public Name Email { get; }
    public Address? Address { get; }

    public User(Guid id, Name username, Name email, Address? address)
    {
        Id = id;
        Username = username;
        Email = email;
        Address = address;
    }
}