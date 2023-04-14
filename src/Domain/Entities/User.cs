using Domain.Primitives;

namespace Domain.Entities;

public record User
{
    public Guid Id { get; }
    public string Email { get; }
    public Address? Address { get; }

    public User(Guid id, string email, Address? address)
    {
        Id = id;
        Email = email;
        Address = address;
    }
}