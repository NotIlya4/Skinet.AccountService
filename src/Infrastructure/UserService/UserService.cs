using Domain.Entities;
using Infrastructure.JwtTokenManager;
using Infrastructure.JwtTokenPairService;
using Infrastructure.UserRepository;
using Infrastructure.UserRepository.Exceptions;
using Infrastructure.UserService.Helpers;
using Infrastructure.UserService.Models;

namespace Infrastructure.UserService;

public class UserService : IUserService
{
    private readonly IJwtTokenService _jwtTokenService;
    private readonly IJwtTokenManager _jwtTokenManager;
    private readonly IUserRepository _repository;
    private readonly IPasswordHasher _passwordHasher;

    public UserService(IJwtTokenService jwtTokenService, IJwtTokenManager jwtTokenManager, IUserRepository repository, IPasswordHasher passwordHasher)
    {
        _jwtTokenService = jwtTokenService;
        _jwtTokenManager = jwtTokenManager;
        _repository = repository;
        _passwordHasher = passwordHasher;
    }

    public async Task<JwtTokenPair> Register(RegisterCredentials registerCredentials)
    {
        User user = new User(Guid.NewGuid(), registerCredentials.Username, registerCredentials.Email);
        await _repository.Insert(user, _passwordHasher.Hash(registerCredentials.Password));
        
        JwtTokenPair jwtPair = await _jwtTokenService.AddNewRefreshToken(user.Id);
        return jwtPair;
    }

    public async Task<JwtTokenPair> Login(LoginCredentials loginCredentials)
    {
        User user = await _repository.Get(UserRepositoryStrictFilter.Email, loginCredentials.Email);
        string passwordHash = await _repository.GetPasswordHash(user.Id);
        
        if (!_passwordHasher.Verify(loginCredentials.Password, passwordHash))
        {
            throw new InvalidOperationException("Wrong password");
        }
        
        JwtTokenPair jwtPair = await _jwtTokenService.AddNewRefreshToken(user.Id);
        return jwtPair;
    }

    public async Task<JwtTokenPair> UpdateJwtPair(JwtTokenPair jwtTokenPair)
    {
        return await _jwtTokenService.UpdatePair(_jwtTokenManager.ExtractUserId(jwtTokenPair.JwtToken), jwtTokenPair.RefreshToken);
    }

    public async Task<User> GetUser(UserServiceStrictFilter filter, string value)
    {
        User? user = null;
        if (filter == UserServiceStrictFilter.Id)
        {
            user = await _repository.Get(UserRepositoryStrictFilter.Id, value);
        }

        if (filter == UserServiceStrictFilter.Email)
        {
            user = await _repository.Get(UserRepositoryStrictFilter.Email, value);
        }

        if (filter == UserServiceStrictFilter.Jwt)
        {
            Guid userId = _jwtTokenManager.ValidateAndExtractUserId(value);
            user = await _repository.Get(UserRepositoryStrictFilter.Id, userId.ToString());
        }
        
        if (user is null)
        {
            throw new InvalidOperationException("User not found");
        }

        return user;
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
            await _repository.Get(UserRepositoryStrictFilter.Email, email);
            return true;
        }
        catch (UserNotFoundException)
        {
            return false;
        }
    }

    public async Task<bool> IsUsernameBusy(string username)
    {
        try
        {
            await _repository.Get(UserRepositoryStrictFilter.Username, username);
            return true;
        }
        catch (UserNotFoundException)
        {
            return false;
        }
    }
}