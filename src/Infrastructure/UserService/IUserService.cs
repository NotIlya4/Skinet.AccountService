using Infrastructure.JwtTokenPairService;
using Infrastructure.UserService.Models;
using Infrastructure.ValidJwtTokenSystem.Models;

namespace Infrastructure.UserService;

public interface IUserService
{
    public Task<JwtTokenPair> Register(RegisterCredentials registerCredentials);
    public Task<JwtTokenPair> Login(LoginCredentials loginCredentials);
    public Task<JwtTokenPair> UpdateJwtPair(JwtTokenPair jwtTokenPair);
    public Task<User> GetUserById(UserId id);
    public Task<User> GetUserByEmail(Email email);
    public Task<User> GetUserByUsername(Username username);
    public Task<User> GetUserByJwtToken(JwtToken jwtToken);
    public Task Logout(JwtTokenPair jwtTokenPair);
    public Task LogOutInAllEntries(UserId userId);
    public Task<bool> IsEmailBusy(Email email);
    public Task<bool> IsUsernameBusy(Username username);
}