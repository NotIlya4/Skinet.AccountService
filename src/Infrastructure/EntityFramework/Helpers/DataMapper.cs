using Domain.Entities;
using Domain.Primitives;
using Domain.Primitives.Address;
using Infrastructure.EntityFramework.Models;

namespace Infrastructure.EntityFramework.Helpers;

public class DataMapper
{
    public UserData MapUser(User user, string passwordHash)
    {
        Address? address = user.Address;
        return new UserData(
            id: user.Id.ToString(),
            username: user.Username.Value,
            email: user.Email.Value,
            passwordHash: passwordHash,
            country: address?.Country.Value,
            city: address?.City.Value,
            street: address?.Street.Value,
            zipcode: address?.Zipcode.Value);
    }

    public User MapUser(UserData userData)
    {
        return new User(
            id: new Guid(userData.Id),
            username: new Username(userData.Username),
            email: new Email(userData.Email),
            address: MapAddress(userData));
    }

    public Address? MapAddress(UserData userData)
    {
        if (userData.Country is null || userData.City is null || userData.Street is null || userData.Zipcode is null)
        {
            return null;
        }

        return new Address(
            country: new Country(userData.Country),
            city: new City(userData.City),
            street: new Street(userData.Street),
            zipcode: new Zipcode(userData.Zipcode));
    }
}