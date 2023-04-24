using System.Diagnostics.CodeAnalysis;
using Domain.Entities;
using Domain.Primitives;
using Infrastructure.EntityFramework.Models;
using Infrastructure.JwtTokenManager;
using Infrastructure.JwtTokenService;
using Infrastructure.UserService.Models;
using Microsoft.AspNetCore.Identity;

namespace Infrastructure.UserService;

public class UserService : IUserService
{
    private readonly IJwtTokenService _jwtTokenService;
    private readonly UserManager<UserData> _userManager;
    private readonly IJwtTokenManager _jwtTokenManager;

    public UserService(IJwtTokenService jwtTokenService, UserManager<UserData> userManager, IJwtTokenManager jwtTokenManager)
    {
        _jwtTokenService = jwtTokenService;
        _userManager = userManager;
        _jwtTokenManager = jwtTokenManager;
    }

    public async Task<JwtTokenPair> Register(RegisterCredentials registerCredentials)
    {
        var userData = new UserData(registerCredentials.Username.Value);
        userData.Email = registerCredentials.Email.Value;
        
        IdentityResult? result = await _userManager.CreateAsync(userData, registerCredentials.Password.Value);
        if (result is null)
        {
            throw new Exception("Result is null");
        }

        if (!result.Succeeded)
        {
            throw new IdentityException(result.Errors);
        }

        return await _jwtTokenService.AddNewRefreshToken(new Guid(userData.Id));
    }

    public async Task<JwtTokenPair> Login(LoginCredentials loginCredentials)
    {
        UserData? user = await _userManager.FindByEmailAsync(loginCredentials.Email.Value);
        
        if (user is null)
        {
            throw new InvalidOperationException("User not found");
        }

        if (!(await _userManager.CheckPasswordAsync(user, loginCredentials.Password.Value)))
        {
            throw new InvalidOperationException("Wrong password");
        }

        return await _jwtTokenService.AddNewRefreshToken(new Guid(user.Id));
    }

    public async Task<JwtTokenPair> UpdateJwtPair(JwtTokenPair jwtTokenPair)
    {
        return await _jwtTokenService.UpdatePair(_jwtTokenManager.ExtractUserId(jwtTokenPair.JwtToken), jwtTokenPair.RefreshToken);
    }

    public async Task<User> GetUser(UserStrictFilterProperty filterProperty, string value)
    {
        UserData? userData = null;
        if (filterProperty == UserStrictFilterProperty.Id)
        {
            userData = await _userManager.FindByIdAsync(value);
        }

        if (filterProperty == UserStrictFilterProperty.Email)
        {
            userData = await _userManager.FindByEmailAsync(value);
        }

        if (filterProperty == UserStrictFilterProperty.Jwt)
        {
            Guid userId = _jwtTokenManager.ValidateAndExtractUserId(value);
            userData = await _userManager.FindByIdAsync(userId.ToString());
        }
        
        if (userData is null)
        {
            throw new InvalidOperationException("User not found");
        }

        return ExtractUser(userData);
    }

    private string ThrowIfNull([NotNull] string? value, string fieldName)
    {
        if (value is null)
        {
            throw new InvalidOperationException($"{fieldName} doesn't exists");
        }

        return value;
    }

    private User ExtractUser(UserData userData)
    {
        Address? address = null;
        try
        {
            address = ExtractAddress(userData);
        }
        catch (InvalidOperationException)
        {
            
        }
        return new User(
            id: new Guid(userData.Id),
            username: new Name(ThrowIfNull(userData.UserName, nameof(userData.UserName))),
            email: new Name(ThrowIfNull(userData.Email, nameof(userData.Email))),
            address: address);
    }

    private Address ExtractAddress(UserData userData)
    {
        return new Address(
            country: ThrowIfNull(userData.Country, nameof(userData.Country)),
            city: ThrowIfNull(userData.City, nameof(userData.City)),
            street: ThrowIfNull(userData.Street, nameof(userData.Street)),
            zipcode: ThrowIfNull(userData.Zipcode, nameof(userData.Zipcode)));
    }

    public async Task Logout(JwtTokenPair jwtTokenPair)
    {
        await _jwtTokenService.ExpireRefreshToken(_jwtTokenManager.ExtractUserId(jwtTokenPair.JwtToken), jwtTokenPair.RefreshToken);
    }

    public async Task LogOutInAllEntries(Guid userId)
    {
        await _jwtTokenService.ExpireAllRefreshTokens(userId);
    }

    public async Task<bool> IsEmailBusy(string email)
    {
        try
        {
            await GetUser(UserStrictFilterProperty.Email, email);
        }
        catch(InvalidOperationException)
        {
            return false;
        }

        return true;
    }
}