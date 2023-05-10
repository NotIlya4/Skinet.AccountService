using Domain.Entities;
using Domain.Primitives;
using Infrastructure.EntityFramework;
using Infrastructure.JwtTokenPairService;
using Infrastructure.RefreshTokenService;
using Infrastructure.UserRepository.Exceptions;
using Infrastructure.UserService;
using Infrastructure.UserService.Models;
using IntegrationTests.Setup;
using Microsoft.Extensions.DependencyInjection;
using StackExchange.Redis;

namespace IntegrationTests.Infrastructure;

[Collection(nameof(AppFixture))]
public class UserServiceRegisterAndLoginTests : IDisposable
{
    private readonly AppFixture _fixture;
    private readonly IUserService _service;
    private readonly IServiceScope _scope;
    private readonly SqlDbHelper _sqlDbHelper;
    private readonly RedisHelper _redisHelper;
    private readonly Username _username = new("Boba");
    private readonly Email _email = new("boba@email.com");
    private readonly Password _password = new("Password1");
    private readonly IRefreshTokenService _refreshTokenService;

    public UserServiceRegisterAndLoginTests(AppFixture fixture)
    {
        _fixture = fixture;
        _scope = fixture.Fixture.Services.CreateScope();
        _service = _scope.ServiceProvider.GetRequiredService<IUserService>();
        _sqlDbHelper = new SqlDbHelper(_scope.ServiceProvider.GetRequiredService<AppDbContext>());
        _redisHelper = new RedisHelper(_scope.ServiceProvider.GetRequiredService<IDatabase>());
        _refreshTokenService = _scope.ServiceProvider.GetRequiredService<IRefreshTokenService>();
    }

    [Fact]
    public async Task Register_RegisterUserWithExistingEmail_Throw()
    {
        await Assert.ThrowsAsync<EmailIsBusyException>(async () => await _service.Register(new RegisterCredentials(
            new Username("asd"), _sqlDbHelper.SampleUser.Email, _sqlDbHelper.SampleUserPassword)));
    }

    [Fact]
    public async Task Register_RegisterUserWithSameUsername_Throw()
    {
        await Assert.ThrowsAsync<UsernameIsBusyException>(async () => await _service.Register(new RegisterCredentials(
            _sqlDbHelper.SampleUser.Username, new Email("a@a"), _sqlDbHelper.SampleUserPassword)));
    }

    [Fact]
    public async Task Register_RegisterNewUser_CreateNewUser()
    {
        await CreateNewUser();

        User result = await _service.GetUserByUsername(new Username("Boba"));
        User expect = new User(result.Id, new Username("Boba"), new Email("boba@email.com"));
        
        Assert.Equal(expect, result);

        await Reload();
    }

    [Fact]
    public async Task Register_GetUserByIdFromProvidedJwt_SameUser()
    {
        JwtTokenPair pair = await CreateNewUser();

        User result = await _service.GetUserById(pair.JwtToken.UserId);
        User expect = new User(result.Id, new Username("Boba"), new Email("boba@email.com"));
        
        Assert.Equal(expect, result);

        await Reload();
    }

    [Fact]
    public async Task Register_RegisterNewUser_RefreshTokenExists()
    {
        JwtTokenPair pair = await CreateNewUser();
        
        Assert.True(await _refreshTokenService.Contains(pair.JwtToken.UserId, pair.RefreshToken));
        
        await Reload();
    }

    [Fact]
    public async Task Login_WrongPassword_Throw()
    {
        await Assert.ThrowsAsync<InvalidOperationException>(async () =>
            await _service.Login(new LoginCredentials(_sqlDbHelper.SampleUser.Email, new Password("PPassword1"))));
    }

    [Fact]
    public async Task Login_WrongEmail_Throw()
    {
        await Assert.ThrowsAsync<UserNotFoundException>(async () =>
            await _service.Login(new LoginCredentials(new Email("aaaaaa@a"), _sqlDbHelper.SampleUserPassword)));
    }

    [Fact]
    public async Task Login_GetUserByIdFromJwt_SameUser()
    {
        JwtTokenPair pair = await _service.Login(new LoginCredentials(_sqlDbHelper.SampleUser.Email, _sqlDbHelper.SampleUserPassword));
        
        User result = await _service.GetUserById(pair.JwtToken.UserId);
        
        Assert.Equal(_sqlDbHelper.SampleUser, result);

        await Reload();
    }

    [Fact]
    public async Task Login_Login_RefreshTokenExists()
    {
        JwtTokenPair pair = await _service.Login(new LoginCredentials(_sqlDbHelper.SampleUser.Email, _sqlDbHelper.SampleUserPassword));
        
        Assert.True(await _refreshTokenService.Contains(_sqlDbHelper.SampleUser.Id, pair.RefreshToken));
        
        await Reload();
    }

    private async Task<JwtTokenPair> CreateNewUser()
    {
        return await _service.Register(new RegisterCredentials(_username, _email, _password));
    }

    public async Task Reload()
    {
        await _sqlDbHelper.Reload();
        await _redisHelper.Reload();
    }
        
    public void Dispose()
    {
        _scope.Dispose();
    }
}