namespace Domain.Primitives;

public readonly record struct Address
{
    public string Country { get; }
    public string City { get; }

    public Address(string country, string city)
    {
        Country = country;
        City = city;
    }
};