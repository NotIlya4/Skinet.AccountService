using Infrastructure.RefreshTokenService;
using Infrastructure.RefreshTokenService.Exceptions;
using Infrastructure.RefreshTokenService.Helpers;
using Infrastructure.RefreshTokenService.Models;
using IntegrationTests.Setup;
using Microsoft.Extensions.DependencyInjection;

namespace IntegrationTests.Infrastructure;

[Collection(nameof(AppFixture))]
public class RefreshTokenServiceTests : IDisposable
{
    private readonly AppFixture _fixture;
    private readonly IRefreshTokenService _service;
    private readonly RedisHelper _helper;
    private readonly IServiceScope _scope;

    public RefreshTokenServiceTests(AppFixture fixture)
    {
        _fixture = fixture;
        _scope = fixture.Fixture.Services.CreateScope();
        _service = _scope.ServiceProvider.GetRequiredService<IRefreshTokenService>();
        _helper = fixture.RedisHelper;
    }

    [Fact]
    public async Task StrictDelete_DeleteTwoTimesSameToken_ThrowOnSecondDelete()
    {
        await StrictDeleteSampleToken();
        
        await AssertThrowsStrictDeleteSampleToken();

        await _helper.Reload();
    }

    [Fact]
    public async Task StrictDelete_TokensExpired_EmptyList()
    {
        var service = new RefreshTokenService(_fixture.Redis, new RefreshTokenSerializer(),
            new RefreshTokenRepositoryOptions(TimeSpan.Zero));

        await Assert.ThrowsAsync<RefreshTokenNotFoundException>(async () =>
            await service.StrictDelete(_helper.SampleUserId, _helper.SampleToken));
        
        await _helper.Reload();
    }

    [Fact]
    public async Task EnsureDeleted_DeleteTwoTimesSameToken_NotThrow()
    {
        await _service.EnsureDeleted(_helper.SampleUserId, _helper.SampleToken);
        await _service.EnsureDeleted(_helper.SampleUserId, _helper.SampleToken);

        await AssertThrowsStrictDeleteSampleToken();

        await _helper.Reload();
    }

    [Fact]
    public async Task DeleteAllForUser_SampleToken_TokenDeleted()
    {
        var token = new Guid("f0d03dbf-1cbc-4198-937c-6fa47241c58e");
        await _service.Add(_helper.SampleUserId, token);
        await _service.DeleteAllForUser(_helper.SampleUserId);

        await AssertThrowsStrictDeleteSampleToken();
        await Assert.ThrowsAsync<RefreshTokenNotFoundException>(async () => await _service.StrictDelete(_helper.SampleUserId, token));
        
        await _helper.Reload();
    }

    [Fact]
    public async Task Add_DeleteAddAndDeleteSameToken_NotThrow()
    {
        await StrictDeleteSampleToken();
        await _service.Add(_helper.SampleUserId, _helper.SampleToken);
        await StrictDeleteSampleToken();

        await _helper.Reload();
    }

    private async Task StrictDeleteSampleToken()
    {
        await _service.StrictDelete(_helper.SampleUserId, _helper.SampleToken);
    }

    private async Task AssertThrowsStrictDeleteSampleToken()
    {
        await Assert.ThrowsAsync<RefreshTokenNotFoundException>(async () => await StrictDeleteSampleToken());
    }

    public void Dispose()
    {
        _scope.Dispose();
    }
}