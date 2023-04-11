using StackExchange.Redis;

namespace Infrastructure.RefreshTokenPersistance;

public class RefreshTokenRepository : IRefreshTokenRepository
{
    private readonly IDatabase _redis;
    private readonly TokenRepositoryOptions _options;

    public RefreshTokenRepository(IDatabase redis, TokenRepositoryOptions options)
    {
        _redis = redis;
        _options = options;
    }

    public async Task Add(Guid userId, Guid token)
    {
        string key = BuildRefreshTokenKey(token);
        string value = userId.ToString();

        await _redis.StringSetAsync(key, value, _options.JwtRefreshTokenExpireTime);
    }

    public async Task<Guid> PopAssociatedUser(Guid refreshToken)
    {
        string key = BuildRefreshTokenKey(refreshToken);

        RedisValue redisValue = await _redis.StringGetDeleteAsync(key);
        
        AssertRedisValueNull(redisValue);

        return new Guid(redisValue.ToString());
    }

    private void AssertRedisValueNull(RedisValue redisValue)
    {
        if (!redisValue.HasValue)
        {
            throw new InvalidRefreshTokenException(
                "There is no such refresh token, its either expired or not even existed");
        }
    }

    private string BuildRefreshTokenKey(Guid refreshToken)
    {
        return $"refresh-token-{refreshToken.ToString()}";
    }
}