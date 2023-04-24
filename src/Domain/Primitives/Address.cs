namespace Domain.Primitives;

public record Address
{
    public Name Country { get; }
    public Name City { get; }
    public Name Street { get; }
    public Name Zipcode { get; }

    public Address(string country, string city, string street, string zipcode) : this(
        country: new Name(country), 
        city: new Name(city), 
        street: new Name(street), 
        zipcode: new Name(zipcode)) { }

    public Address(Name country, Name city, Name street, Name zipcode)
    {
        Country = country;
        City = city;
        Street = street;
        Zipcode = zipcode;
    }
};