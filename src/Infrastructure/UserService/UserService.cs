using Domain.Exceptions;
using Domain.Primitives;
using Infrastructure.JwtTokenService;
using Microsoft.AspNetCore.Identity;

namespace Infrastructure.UserService;

public class UserService : IUserService
{
    private readonly IJwtTokenService _jwtTokenService;
    private readonly UserManager<IdentityUser> _userManager;

    public UserService(IJwtTokenService jwtTokenService, UserManager<IdentityUser> userManager)
    {
        _jwtTokenService = jwtTokenService;
        _userManager = userManager;
    }
    
    public async Task<JwtTokenPair> Register(string email, string password)
    {
        IdentityUser user = new(email);
        
        user.Email = email;
        
        IdentityResult? result = await _userManager.CreateAsync(user, password);
        
        if (result is null)
        {
            throw new Exception("Result is null");
        }

        if (!result.Succeeded)
        {
            throw new IdentityException(result.Errors);
        }

        return await _jwtTokenService.AddNewRefreshToken(new UserId(user.Id));
    }

    public async Task<JwtTokenPair> Login(string email, string password)
    {
        IdentityUser? user = await _userManager.FindByEmailAsync(email);
        
        if (user is null)
        {
            throw new ValidationException("User not found");
        }

        if (!(await _userManager.CheckPasswordAsync(user, password)))
        {
            throw new ValidationException("Wrong password");
        }

        return await _jwtTokenService.AddNewRefreshToken(new UserId(user.Id));
    }

    public async Task Logout(UserId userId, RefreshToken refreshToken)
    {
        await _jwtTokenService.ExpireRefreshToken(userId, refreshToken);
    }

    public async Task LogOutInAllEntries(UserId userId)
    {
        await _jwtTokenService.ExpireAllRefreshTokens(userId);
    }

    public async Task<JwtTokenPair> UpdateJwtPair(UserId userId, RefreshToken refreshToken)
    {
        return await _jwtTokenService.UpdatePair(userId, refreshToken);
    }

    public void ValidateJwtToken(string jwtToken)
    {
        _jwtTokenService.ValidateJwtToken(jwtToken);
    }
}