using System.Diagnostics.CodeAnalysis;
using Domain.Entities;
using Domain.Primitives;
using Infrastructure.EntityFramework.Models;
using Infrastructure.JwtTokenService;
using Microsoft.AspNetCore.Identity;

namespace Infrastructure.UserService;

public class UserService : IUserService
{
    private readonly IJwtTokenService _jwtTokenService;
    private readonly UserManager<UserData> _userManager;

    public UserService(IJwtTokenService jwtTokenService, UserManager<UserData> userManager)
    {
        _jwtTokenService = jwtTokenService;
        _userManager = userManager;
    }

    public async Task<User> GetUser(UserStrictFilterProperty filterProperty, string value)
    {
        if (filterProperty == UserStrictFilterProperty.Id)
        {
            UserData? userData = await _userManager.FindByIdAsync(value);
            if (userData is null)
            {
                throw new InvalidOperationException("User not found");
            }

            return ExtractUser(userData);
        }

        if (filterProperty == UserStrictFilterProperty.Email)
        {
            UserData? userData = await _userManager.FindByEmailAsync(value);
            if (userData is null)
            {
                throw new InvalidOperationException("User not found");
            }

            return ExtractUser(userData);
        }

        throw new InvalidOperationException("Id and Email supported only");
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
        return new(
            id: new Guid(userData.Id),
            email: ThrowIfNull(userData.Email, nameof(userData.Email)),
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

    public async Task<JwtTokenPair> Register(string email, string password)
    {
        UserData userData = new(email);
        userData.Email = email;
        
        IdentityResult? result = await _userManager.CreateAsync(userData, password);
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

    public async Task<JwtTokenPair> Login(string email, string password)
    {
        UserData? user = await _userManager.FindByEmailAsync(email);
        
        if (user is null)
        {
            throw new InvalidOperationException("User not found");
        }

        if (!(await _userManager.CheckPasswordAsync(user, password)))
        {
            throw new InvalidOperationException("Wrong password");
        }

        return await _jwtTokenService.AddNewRefreshToken(new Guid(user.Id));
    }

    public async Task Logout(Guid userId, Guid refreshToken)
    {
        await _jwtTokenService.ExpireRefreshToken(userId, refreshToken);
    }

    public async Task LogOutInAllEntries(Guid userId)
    {
        await _jwtTokenService.ExpireAllRefreshTokens(userId);
    }

    public async Task<JwtTokenPair> UpdateJwtPair(Guid userId, Guid refreshToken)
    {
        return await _jwtTokenService.UpdatePair(userId, refreshToken);
    }

    public void ValidateJwtToken(string jwtToken)
    {
        _jwtTokenService.ValidateJwtToken(jwtToken);
    }
}