namespace Domain.Primitives.Address;

public record Address
{
    public Country Country { get; }
    public City City { get; }
    public Street Street { get; }
    public Zipcode Zipcode { get; }

    public Address(string country, string city, string street, string zipcode) : this(
        country: new Country(country), 
        city: new City(city), 
        street: new Street(street), 
        zipcode: new Zipcode(zipcode)) { }

    public Address(Country country, City city, Street street, Zipcode zipcode)
    {
        Country = country;
        City = city;
        Street = street;
        Zipcode = zipcode;
    }
};