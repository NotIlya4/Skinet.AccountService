namespace Domain.Primitives;

public record Address
{
    public string Country { get; }
    public string City { get; }
    public string Street { get; }
    public string Zipcode { get; }

    public Address(string country, string city, string street, string zipcode)
    {
        Country = country;
        City = city;
        Street = street;
        Zipcode = zipcode;
    }
};