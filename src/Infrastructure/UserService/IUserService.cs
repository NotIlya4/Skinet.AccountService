using Domain.Primitives;
using Infrastructure.JwtTokenService;

namespace Infrastructure.UserService;

public interface IUserService
{
    public Task<JwtTokenPair> Register(string email, string password);
    public Task<JwtTokenPair> Login(string email, string password);
    public Task Logout(UserId userId, RefreshToken refreshToken);
    public Task LogOutInAllEntries(UserId userId);
    public Task<JwtTokenPair> UpdateJwtPair(UserId userId, RefreshToken refreshToken);
    public void ValidateJwtToken(string jwtToken);
}