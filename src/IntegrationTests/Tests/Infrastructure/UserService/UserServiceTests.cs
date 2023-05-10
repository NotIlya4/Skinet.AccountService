using Domain.Entities;
using Domain.Primitives;
using Infrastructure.EntityFramework;
using Infrastructure.JwtTokenHelper;
using Infrastructure.JwtTokenPairService;
using Infrastructure.RefreshTokenService;
using Infrastructure.RefreshTokenService.Exceptions;
using Infrastructure.RefreshTokenService.Models;
using Infrastructure.UserRepository.Exceptions;
using Infrastructure.UserService;
using Infrastructure.UserService.Models;
using IntegrationTests.Setup;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using StackExchange.Redis;

namespace IntegrationTests.Infrastructure;

[Collection(nameof(AppFixture))]
public class UserServiceTests : IDisposable
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

    public UserServiceTests(AppFixture fixture)
    {
        _fixture = fixture;
        _scope = fixture.Fixture.Services.CreateScope();
        _service = _scope.ServiceProvider.GetRequiredService<IUserService>();
        _sqlDbHelper = new SqlDbHelper(_scope.ServiceProvider.GetRequiredService<AppDbContext>());
        _redisHelper = new RedisHelper(_scope.ServiceProvider.GetRequiredService<IDatabase>());
        _refreshTokenService = _scope.ServiceProvider.GetRequiredService<IRefreshTokenService>();
    }

    [Fact]
    public async Task UpdateJwtPair_InvalidJwtToken_Throw()
    {
        string jwt = "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJzdWIiOiI2MGY2MTAzYS1kZjlmLTQyNDEtYTU0ZC1hN2Y2MWQ5YjllOGUiLCJuYW1lIjoiSm9obiBEb2UiLCJpYXQiOjE1MTYyMzkwMjIsImV4cCI6MTUxNjIzOTEyMn0.ho9RPeKtMqINoXqw7LtqlOKawuumNBTtRnEtOyRiJ_A";
        var pair = new JwtTokenPair(new JwtToken(jwt), new RefreshToken(Guid.NewGuid()));
        await Assert.ThrowsAnyAsync<SecurityTokenException>(async () => await _service.UpdateJwtPair(pair));
    }

    [Fact]
    public async Task UpdateJwtPair_Update_DeletePreviousRefreshToken()
    {
        JwtTokenPair pair = await Login();
        await _service.UpdateJwtPair(pair);
        
        Assert.False(await _refreshTokenService.Contains(_sqlDbHelper.SampleUser.Id, pair.RefreshToken));

        await Reload();
    }

    [Fact]
    public async Task GetUserById_Get_UserWithSpecifiedId()
    {
        User result = await _service.GetUserById(_sqlDbHelper.SampleUser.Id);
        
        Assert.Equal(_sqlDbHelper.SampleUser, result);
    }
    
    [Fact]
    public async Task GetUserByEmail_Get_UserWithSpecifiedEmail()
    {
        User result = await _service.GetUserByEmail(_sqlDbHelper.SampleUser.Email);
        
        Assert.Equal(_sqlDbHelper.SampleUser, result);
    }
    
    [Fact]
    public async Task GetUserByUsername_Get_UserWithSpecifiedUsername()
    {
        User result = await _service.GetUserByUsername(_sqlDbHelper.SampleUser.Username);
        
        Assert.Equal(_sqlDbHelper.SampleUser, result);
    }
    
    [Fact]
    public async Task GetUserByJwt_Get_UserWithIdFromJwt()
    {
        JwtTokenPair pair = await Login();
        
        User result = await _service.GetUserByJwtToken(pair.JwtToken);
        
        Assert.Equal(_sqlDbHelper.SampleUser, result);
    }

    private async Task<JwtTokenPair> Login()
    {
        return await _service.Login(new LoginCredentials(_sqlDbHelper.SampleUser.Email, _sqlDbHelper.SampleUserPassword));
    }

    private async Task Reload()
    {
        await _sqlDbHelper.Reload();
        await _redisHelper.Reload();
    }
        
    public void Dispose()
    {
        _scope.Dispose();
    }
}