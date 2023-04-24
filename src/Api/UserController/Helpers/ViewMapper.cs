using Api.UserController.Views;
using Domain.Entities;
using Domain.Primitives.Address;
using Infrastructure.JwtTokenSystem.Service;
using Infrastructure.UserSystem.Service.Models;

namespace Api.UserController.Helpers;

public class ViewMapper
{
    public JwtTokenPairView MapJwtTokenPair(JwtTokenPair jwtTokenPair)
    {
        return new JwtTokenPairView(
            jwtToken: jwtTokenPair.JwtToken,
            refreshToken: jwtTokenPair.RefreshToken.ToString());
    }

    public JwtTokenPair MapJwtTokenPair(JwtTokenPairView view)
    {
        return new JwtTokenPair(
            jwtToken: view.JwtToken,
            refreshToken: new Guid(view.RefreshToken));
    }

    public UserView MapUser(User user)
    {
        if (user.Address is null)
        {
            return new UserView(
                id: user.Id.ToString(),
                username: user.Username.Value,
                email: user.Email.Value);
        }
        
        return new UserView(
            id: user.Id.ToString(),
            username: user.Username.Value,
            email: user.Email.Value,
            address: MapAddress(user.Address));
    }

    public AddressView MapAddress(Address address)
    {
        return new AddressView(
            country: address.Country.Value,
            city: address.City.Value,
            street: address.Street.Value,
            zipcode: address.Zipcode.Value);
    }

    public RegisterCredentials MapRegisterCredentials(RegisterCredentialsView view)
    {
        return new RegisterCredentials(
            username: view.Username,
            email: view.Email,
            password: view.Password);
    }

    public LoginCredentials MapLoginCredentials(LoginCredentialsView view)
    {
        return new LoginCredentials(
            email: view.Email,
            password: view.Password);
    }
}