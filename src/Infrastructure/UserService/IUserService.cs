using Domain.Entities;
using Infrastructure.JwtTokenService;

namespace Infrastructure.UserService;

public interface IUserService
{
    public Task<JwtTokenPair> Register(string email, string password);
    public Task<JwtTokenPair> Login(string email, string password);
    public Task<JwtTokenPair> UpdateJwtPair(JwtTokenPair jwtTokenPair);
    public Task<User> GetUser(string jwtToken);
    public Task<User> GetUser(UserStrictFilterProperty filterProperty, string value);
    public Task Logout(JwtTokenPair jwtTokenPair);
    public Task LogOutInAllEntries(Guid userId);
    public Task<bool> IsEmailBusy(string email);
}