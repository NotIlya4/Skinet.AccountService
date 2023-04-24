using Domain.Entities;
using Infrastructure.JwtTokenSystem.Service;
using Infrastructure.UserSystem.Service.Models;

namespace Infrastructure.UserSystem.Service;

public interface IUserService
{
    public Task<JwtTokenPair> Register(RegisterCredentials registerCredentials);
    public Task<JwtTokenPair> Login(LoginCredentials loginCredentials);
    public Task<JwtTokenPair> UpdateJwtPair(JwtTokenPair jwtTokenPair);
    public Task<User> GetUser(UserServiceStrictFilter filter, string value);
    public Task Logout(JwtTokenPair jwtTokenPair);
    public Task LogOutInAllEntries(Guid userId);
    public Task<bool> IsEmailBusy(string email);
    public Task<bool> IsUsernameBusy(string username);
}