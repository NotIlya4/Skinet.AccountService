using System.Security.Claims;
using Domain.Exceptions;
using Domain.Primitives;
using Infrastructure.JwtTokenService;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.JsonWebTokens;

namespace Infrastructure.UserService;

public class UserService : IUserService
{
    private readonly IJwtTokenService _jwtTokenService;
    private readonly UserManager<IdentityUser> _userManager;
    private readonly SignInManager<IdentityUser> _signInManager;

    public UserService(IJwtTokenService jwtTokenService, UserManager<IdentityUser> userManager, SignInManager<IdentityUser> signInManager)
    {
        _jwtTokenService = jwtTokenService;
        _userManager = userManager;
        _signInManager = signInManager;
    }
    
    public async Task<JwtTokenPair> Register(BasicRegisterCredentials registerCredentials)
    {
        IdentityUser user = new(registerCredentials.Email);
        
        user.Email = registerCredentials.Email;
        
        IdentityResult? result = await _userManager.CreateAsync(user, registerCredentials.Password);
        
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

    public async Task<JwtTokenPair> Login(BasicRegisterCredentials registerCredentials)
    {
        SignInResult? result = await _signInManager.PasswordSignInAsync(
            registerCredentials.Email, 
            registerCredentials.Password, false, false);
        
        if (result is null)
        {
            throw new Exception("Result is null");
        }

        if (!result.Succeeded)
        {
            throw new ValidationException("Wrong login credentials");
        }

        IdentityUser? user = await _userManager.FindByEmailAsync(registerCredentials.Email);
        if (user is null)
        {
            throw new ValidationException("User doesn't exists");
        }

        return await _jwtTokenService.AddNewRefreshToken(new UserId(user.Id));
    }

    public async Task LogOut(UserId userId, RefreshToken refreshToken)
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
}