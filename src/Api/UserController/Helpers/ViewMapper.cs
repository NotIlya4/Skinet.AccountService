using Api.UserController.Views;
using Domain.Entities;
using Domain.Primitives;
using Infrastructure.JwtTokenService;

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
                email: user.Email);
        }
        
        return new UserView(
            id: user.Id.ToString(),
            email: user.Email,
            address: MapAddress(user.Address));
    }

    public AddressView MapAddress(Address address)
    {
        return new AddressView(
            country: address.Country,
            city: address.City,
            street: address.Street,
            zipcode: address.Zipcode);
    }
}