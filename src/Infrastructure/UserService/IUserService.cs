using Domain.Entities;
using Infrastructure.JwtTokenService;
using Infrastructure.UserService.Models;

namespace Infrastructure.UserService;

public interface IUserService
{
    public Task<JwtTokenPair> Register(RegisterCredentials registerCredentials);
    public Task<JwtTokenPair> Login(LoginCredentials loginCredentials);
    public Task<JwtTokenPair> UpdateJwtPair(JwtTokenPair jwtTokenPair);
    public Task<User> GetUser(UserStrictFilterProperty filterProperty, string value);
    public Task Logout(JwtTokenPair jwtTokenPair);
    public Task LogOutInAllEntries(Guid userId);
    public Task<bool> IsEmailBusy(string email);
}