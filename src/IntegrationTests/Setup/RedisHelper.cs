using StackExchange.Redis;

namespace IntegrationTests.Setup;

public class RedisHelper
{
    public UserId SampleUserId { get; } = new("d49ad24a-7437-4c3c-bdb7-c4cb4e08a845");
    private readonly IDatabase _redis;

    public RedisHelper(IDatabase redis)
    {
        _redis = redis;
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