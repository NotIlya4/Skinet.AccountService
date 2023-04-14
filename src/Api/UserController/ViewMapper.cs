using Api.UserController.Views;
using Domain.Entities;
using Infrastructure.JwtTokenService;

namespace Api.UserController;

public class ViewMapper
{
    public JwtTokenPairView MapJwtTokenPair(JwtTokenPair jwtTokenPair)
    {
        return new JwtTokenPairView()
        {
            JwtToken = jwtTokenPair.JwtToken,
            RefreshToken = jwtTokenPair.RefreshToken.ToString()
        };
    }

    public JwtTokenPair MapJwtTokenPair(JwtTokenPairView jwtTokenPairView)
    {
        return new JwtTokenPair()
        {
            JwtToken = jwtTokenPairView.JwtToken,
            RefreshToken = new Guid(jwtTokenPairView.RefreshToken)
        };
    }

    public UserView MapUser(User user)
    {
        if (user.Address is null)
        {
            return new UserView()
            {
                Id = user.Id.ToString(),
                Email = user.Email,
            };
        }
        
        return new UserView()
        {
            Id = user.Id.ToString(),
            Email = user.Email,
            Country = user.Address.City,
            City = user.Address.City,
            Street = user.Address.Street,
            Zipcode = user.Address.Zipcode
        };
    }
}