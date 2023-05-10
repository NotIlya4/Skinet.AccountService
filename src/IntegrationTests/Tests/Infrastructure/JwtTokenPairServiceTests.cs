using Infrastructure.JwtTokenHelper;
using Infrastructure.JwtTokenPairService;
using Infrastructure.RefreshTokenService;
using Infrastructure.RefreshTokenService.Exceptions;
using Infrastructure.RefreshTokenService.Models;
using IntegrationTests.Setup;
using Microsoft.Extensions.DependencyInjection;

namespace IntegrationTests.Infrastructure;

[Collection(nameof(AppFixture))]
public class JwtTokenPairServiceTests : IDisposable
{
    private readonly RedisHelper _helper;
    private readonly IJwtTokenPairService _service;
    private readonly IServiceScope _scope;
    private readonly UserId _userId;
    private readonly IJwtTokenHelper _jwtTokenHelper;
    private readonly IRefreshTokenService _refreshTokenService;

    public JwtTokenPairServiceTests(AppFixture fixture)
    {
        _helper = new RedisHelper(fixture.Redis);
        _scope = fixture.Fixture.Services.CreateScope();
        _service = _scope.ServiceProvider.GetRequiredService<IJwtTokenPairService>();
        _userId = _helper.SampleUserId;
        _jwtTokenHelper = _scope.ServiceProvider.GetRequiredService<IJwtTokenHelper>();
        _refreshTokenService = _scope.ServiceProvider.GetRequiredService<IRefreshTokenService>();
    }

    [Fact]
    public async Task CreateNewPair_UserId_JwtTokenWithThatId()
    {
        JwtTokenPair tokenPair = await _service.CreateNewPair(_userId);
        
        Assert.Equal(_userId, tokenPair.JwtToken.UserId);

        await _helper.Reload();
    }

    [Fact]
    public async Task CreateNewPair_CreatePair_RefreshTokenConsistInStorage()
    {
        JwtTokenPair pair = await _service.CreateNewPair(_userId);
        
        Assert.True(await _refreshTokenService.Contains(_userId, pair.RefreshToken));

        await _helper.Reload();
    }

    [Fact]
    public async Task UpdatePair_UpdatePair_RefreshTokenDeleted()
    {
        JwtTokenPair pair = await _service.CreateNewPair(_userId);
        
        await _service.UpdatePair(_userId, pair.RefreshToken);
        
        Assert.False(await _refreshTokenService.Contains(_userId, pair.RefreshToken));
        
        await _helper.Reload();
    }

    [Fact]
    public async Task UpdatePair_UpdatePairTwoTimes_ThrowOnSecondUpdate()
    {
        JwtTokenPair pair = await _service.CreateNewPair(_userId);
        
        await _service.UpdatePair(_userId, pair.RefreshToken);
        await Assert.ThrowsAsync<RefreshTokenNotFoundException>(async () => await _service.UpdatePair(_userId, pair.RefreshToken));
        
        await _helper.Reload();
    }

    public void Dispose()
    {
        _scope.Dispose();
    }
}