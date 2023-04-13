using Domain.Primitives;
using Infrastructure.JwtTokenService;

namespace Infrastructure.UserService;

public interface IUserService
{
    public Task<JwtTokenPair> Register(BasicRegisterCredentials registerCredentials);
    public Task<JwtTokenPair> Login(BasicRegisterCredentials registerCredentials);
    public Task LogOut(UserId userId, RefreshToken refreshToken);
    public Task LogOutInAllEntries(UserId userId);
    public Task<JwtTokenPair> UpdateJwtPair(UserId userId, RefreshToken refreshToken);
}