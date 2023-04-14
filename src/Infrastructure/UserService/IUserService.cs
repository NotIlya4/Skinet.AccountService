using Domain.Entities;
using Infrastructure.JwtTokenService;

namespace Infrastructure.UserService;

public interface IUserService
{
    public Task<User> GetUser(UserStrictFilterProperty filterProperty, string value);
    public Task<JwtTokenPair> Register(string email, string password);
    public Task<JwtTokenPair> Login(string email, string password);
    public Task Logout(Guid userId, Guid refreshToken);
    public Task LogOutInAllEntries(Guid userId);
    public Task<JwtTokenPair> UpdateJwtPair(Guid userId, Guid refreshToken);
    public void ValidateJwtToken(string jwtToken);
}