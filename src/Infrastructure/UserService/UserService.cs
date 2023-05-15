using Infrastructure.JwtTokenHelper;
using Infrastructure.JwtTokenPairService;
using Infrastructure.UserRepository;
using Infrastructure.UserRepository.Exceptions;
using Infrastructure.UserService.Helpers;
using Infrastructure.UserService.Models;
using Infrastructure.ValidJwtTokenSystem.Models;
using Microsoft.Extensions.Logging;
using Serilog;
using Serilog.Context;

namespace Infrastructure.UserService;

public class UserService : IUserService
{
    private readonly IJwtTokenPairService _jwtTokenPairService;
    private readonly IJwtTokenHelper _jwtTokenHelper;
    private readonly IUserRepository _repository;
    private readonly IPasswordHasher _passwordHasher;
    private readonly ILogger<UserService> _logger;

    public UserService(IJwtTokenPairService jwtTokenPairService, IJwtTokenHelper jwtTokenHelper, 
        IUserRepository repository, IPasswordHasher passwordHasher, ILogger<UserService> logger)
    {
        _jwtTokenPairService = jwtTokenPairService;
        _jwtTokenHelper = jwtTokenHelper;
        _repository = repository;
        _passwordHasher = passwordHasher;
        _logger = logger;
    }

    public async Task<JwtTokenPair> Register(RegisterCredentials registerCredentials)
    {
        User user = new User(new UserId(Guid.NewGuid()), registerCredentials.Username, registerCredentials.Email);
        await _repository.Insert(user, _passwordHasher.Hash(registerCredentials.Password));
        
        JwtTokenPair jwtPair = await _jwtTokenPairService.CreateNewPair(user.Id);
        using (LogContext.PushProperty("User", user))
        {
            _logger.LogInformation("New user registered");
        }

        return jwtPair;
    }

    public async Task<JwtTokenPair> Login(LoginCredentials loginCredentials)
    {
        User user = await _repository.GetByEmail(loginCredentials.Email);
        string passwordHash = await _repository.GetPasswordHash(user.Id);
        
        if (!_passwordHasher.Verify(loginCredentials.Password, passwordHash))
        {
            throw new InvalidOperationException("Wrong password");
        }
        using (LogContext.PushProperty("User", user))
        {
            _logger.LogInformation("User logged in");
        }
        
        JwtTokenPair jwtPair = await _jwtTokenPairService.CreateNewPair(user.Id);
        return jwtPair;
    }

    public async Task<JwtTokenPair> UpdateJwtPair(JwtTokenPair jwtTokenPair)
    {
        return await _jwtTokenPairService.UpdatePair(jwtTokenPair.JwtToken.UserId, jwtTokenPair.RefreshToken);
    }

    public async Task<User> GetUserById(UserId id)
    {
        return await _repository.GetById(id);
    }

    public async Task<User> GetUserByEmail(Email email)
    {
        return await _repository.GetByEmail(email);
    }

    public async Task<User> GetUserByUsername(Username username)
    {
        return await _repository.GetByUsername(username);
    }

    public async Task<User> GetUserByJwtToken(JwtToken jwtToken)
    {
        ValidJwtToken token = _jwtTokenHelper.Validate(jwtToken);
        return await _repository.GetById(token.Token.UserId);
    }

    public async Task Logout(JwtTokenPair jwtTokenPair)
    {
        ValidJwtToken token = _jwtTokenHelper.Validate(jwtTokenPair.JwtToken);
        await _jwtTokenPairService.EnsureRefreshTokenExpired(token.Token.UserId, jwtTokenPair.RefreshToken);
    }

    public async Task LogOutInAllEntries(UserId userId)
    {
        await _jwtTokenPairService.ExpireAllRefreshTokens(userId);
    }

    public async Task<bool> IsEmailBusy(Email email)
    {
        try
        {
            await _repository.GetByEmail(email);
            return true;
        }
        catch (UserNotFoundException)
        {
            return false;
        }
    }

    public async Task<bool> IsUsernameBusy(Username username)
    {
        try
        {
            await _repository.GetByUsername(username);
            return true;
        }
        catch (UserNotFoundException)
        {
            return false;
        }
    }
}