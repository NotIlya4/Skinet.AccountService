using Infrastructure.RefreshTokenService;
using Infrastructure.RefreshTokenService.Helpers;
using Infrastructure.RefreshTokenService.Models;
using StackExchange.Redis;

namespace IntegrationTests.Setup;

public class RedisHelper
{
    public Guid SampleUserId { get; } = new Guid("d49ad24a-7437-4c3c-bdb7-c4cb4e08a845");
    public Guid SampleToken { get; } = new Guid("4615379b-eb72-4a66-a99b-d841d7c93cea");
    private readonly IDatabase _redis;
    private readonly IRefreshTokenService _service;

    public RedisHelper(IDatabase redis)
    {
        _redis = redis;
        _service = new RefreshTokenService(redis, new RefreshTokenSerializer(),
            new RefreshTokenRepositoryOptions(TimeSpan.FromDays(1)));
    }

    public async Task Reload()
    {
        await Drop();
        await Seed();
    }

    public async Task Seed()
    {
        await _service.Add(SampleUserId, SampleToken);
    }

    public async Task Drop()
    {
        await _redis.ExecuteAsync("FLUSHDB");
    }
}