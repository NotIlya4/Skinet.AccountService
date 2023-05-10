using Infrastructure.RefreshTokenService;
using Infrastructure.RefreshTokenService.Helpers;
using Infrastructure.RefreshTokenService.Models;
using StackExchange.Redis;

namespace IntegrationTests.Setup;

public class RedisHelper
{
    public UserId SampleUserId { get; } = new("d49ad24a-7437-4c3c-bdb7-c4cb4e08a845");
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
    }

    public async Task Drop()
    {
        await _redis.ExecuteAsync("FLUSHDB");
    }
}