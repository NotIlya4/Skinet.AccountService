using Api.UserController.Views;
using Domain.Entities;
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
        return new UserView(
            id: user.Id.ToString(),
            username: user.Username.Value,
            email: user.Email.Value);
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