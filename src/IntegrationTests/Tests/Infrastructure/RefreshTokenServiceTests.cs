using Infrastructure.RefreshTokenService;
using Infrastructure.RefreshTokenService.Exceptions;
using Infrastructure.RefreshTokenService.Models;
using IntegrationTests.Setup;
using Microsoft.Extensions.DependencyInjection;

namespace IntegrationTests.Tests.Infrastructure;

[Collection(nameof(AppFixture))]
public class RefreshTokenServiceTests : IDisposable
{
    private readonly IRefreshTokenService _service;
    private readonly RedisHelper _helper;
    private readonly IServiceScope _scope;
    private readonly UserId _userId;

    public RefreshTokenServiceTests(AppFixture fixture)
    {
        _scope = fixture.Fixture.Services.CreateScope();
        _service = _scope.ServiceProvider.GetRequiredService<IRefreshTokenService>();
        _helper = fixture.RedisHelper;
        _userId = _helper.SampleUserId;
    }

    [Fact]
    public async Task CreateNew_CreateNew_Contain()
    {
        RefreshToken newToken = await _service.CreateNew(_userId);
        
        bool result = await _service.Contains(_userId, newToken);
        
        Assert.True(result);
        
        await _helper.Reload();
    }

    [Fact]
    public async Task StrictDelete_DeleteTwoTimesSameToken_ThrowOnSecondDelete()
    {
        RefreshToken token = await _service.CreateNew(_userId);
        
        await _service.StrictDelete(_userId, token);
        
        await Assert.ThrowsAsync<RefreshTokenNotFoundException>(async () =>
            await _service.StrictDelete(_userId, token));

        await _helper.Reload();
    }

    [Fact]
    public async Task EnsureDeleted_DeleteExistingToken_NotContainsToken()
    {
        RefreshToken token = await _service.CreateNew(_userId);
        
        await _service.EnsureDeleted(_userId, token);
        Assert.False(await _service.Contains(_userId, token));

        await _helper.Reload();
    }

    [Fact]
    public async Task EnsureDeleted_DeleteTwoTimesSameToken_NotThrow()
    {
        RefreshToken token = await _service.CreateNew(_userId);
        
        await _service.EnsureDeleted(_userId, token);
        await _service.EnsureDeleted(_userId, token);

        await _helper.Reload();
    }

    [Fact]
    public async Task EnsureDeleted_DeleteTokenThatNotExists_NotThrow()
    {
        await _service.EnsureDeleted(_userId, new RefreshToken(Guid.NewGuid()));

        await _helper.Reload();
    }

    [Fact]
    public async Task DeleteAllForUser_TwoTokens_TokensDeleted()
    {
        RefreshToken token1 = await _service.CreateNew(_userId);
        RefreshToken token2 = await _service.CreateNew(_userId);
        
        await _service.DeleteAllForUser(_userId);

        Assert.False(await _service.Contains(_userId, token1));
        Assert.False(await _service.Contains(_userId, token2));
        
        await _helper.Reload();
    }

    [Fact]
    public async Task Contains_CreatedToken_True()
    {
        RefreshToken token = await _service.CreateNew(_userId);
        
        Assert.True(await _service.Contains(_userId, token));
    }
    
    [Fact]
    public async Task Contains_NotExistingToken_False()
    {
        Assert.False(await _service.Contains(_userId, new RefreshToken(Guid.NewGuid())));
    }

    public void Dispose()
    {
        _scope.Dispose();
    }
}