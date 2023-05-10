using Api.UserController.Views;
using Domain.Entities;
using Domain.Primitives;
using Infrastructure.JwtTokenHelper;
using Infrastructure.JwtTokenPairService;
using Infrastructure.RefreshTokenService.Models;
using Infrastructure.UserService.Models;

namespace Api.UserController.Helpers;

public class ViewMapper
{
    public JwtTokenPairView MapJwtTokenPair(JwtTokenPair jwtTokenPair)
    {
        return new JwtTokenPairView(
            jwtToken: jwtTokenPair.JwtToken.Raw,
            refreshToken: jwtTokenPair.RefreshToken.Value.ToString());
    }

    public JwtTokenPair MapJwtTokenPair(JwtTokenPairView view)
    {
        return new JwtTokenPair(
            jwtToken: new JwtToken(view.JwtToken),
            refreshToken: new RefreshToken(new Guid(view.RefreshToken)));
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
            email: new Email(view.Email),
            password: new Password(view.Password));
    }
}