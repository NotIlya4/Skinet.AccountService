using System.IdentityModel.Tokens.Jwt;
using Infrastructure.JwtTokenPairService;
using Infrastructure.RefreshTokenService.Exceptions;
using IntegrationTests.Setup;
using Microsoft.Extensions.DependencyInjection;

namespace IntegrationTests.Infrastructure;

[Collection(nameof(AppFixture))]
public class JwtTokenServiceTests : IDisposable
{
    private readonly RedisHelper _helper;
    private readonly IJwtTokenService _service;
    private readonly IServiceScope _scope;
    

    public JwtTokenServiceTests(AppFixture fixture)
    {
        _helper = new RedisHelper(fixture.Redis);
        _scope = fixture.Fixture.Services.CreateScope();
        _service = _scope.ServiceProvider.GetRequiredService<IJwtTokenService>();
    }

    [Fact]
    public async Task CreateNewPair_UserId_JwtTokenWithThatId()
    {
        JwtTokenPair tokenPair = await _service.CreateNewPair(_helper.SampleUserId);
        
        string result = new JwtSecurityToken(tokenPair.JwtToken).Subject;
        
        Assert.Equal(_helper.SampleUserId.ToString(), result);

        await _helper.Reload();
    }

    [Fact]
    public async Task CreateNewPair_CreatePairThenExpireTokenTwoTimes_OnSecondTimeThrow()
    {
        JwtTokenPair pair = await _service.CreateNewPair(_helper.SampleUserId);
        await _service.ExpireRefreshToken(_helper.SampleUserId, pair.RefreshToken);

        await AssertTokenExpired(_helper.SampleUserId, pair.RefreshToken);
        
        await _helper.Reload();
    }

    [Fact]
    public async Task ExpireRefreshToken_ExpireSampleTokenTwoTimes_OnSecondTimeThrow()
    {
        await _service.ExpireRefreshToken(_helper.SampleUserId, _helper.SampleToken);
        await AssertTokenExpired(_helper.SampleUserId, _helper.SampleToken);
        
        await _helper.Reload();
    }

    [Fact]
    public async Task UpdatePair_UpdateSamplePair_UpdateAgainThrow()
    {
        await _service.UpdatePair(_helper.SampleUserId, _helper.SampleToken);
        await AssertTokenExpired(_helper.SampleUserId, _helper.SampleToken);
        
        await _helper.Reload();
    }

    [Fact]
    public async Task ExpireAllRefreshTokens_CreateNewPairThenExpireAllAndTryToExpireThem_Throw()
    {
        JwtTokenPair pair = await _service.CreateNewPair(_helper.SampleUserId);
        await _service.ExpireAllRefreshTokens(_helper.SampleUserId);

        await AssertTokenExpired(_helper.SampleUserId, pair.RefreshToken);
        await AssertTokenExpired(_helper.SampleUserId, _helper.SampleToken);
        
        await _helper.Reload();
    }

    private async Task AssertTokenExpired(Guid userId, Guid refreshToken)
    {
        await Assert.ThrowsAsync<RefreshTokenNotFoundException>(async () =>
            await _service.UpdatePair(userId, refreshToken));
    }

    public void Dispose()
    {
        _scope.Dispose();
    }
}