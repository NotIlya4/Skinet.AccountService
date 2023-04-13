using Domain.Primitives;
using Newtonsoft.Json.Linq;
using StackExchange.Redis;

namespace Infrastructure.RefreshTokenSystem.Repository;

public class RefreshTokenRepository : IRefreshTokenRepository
{
    private readonly IDatabase _redis;
    private readonly RefreshTokenSerializer _serializer;
    private readonly RefreshTokenRepositoryOptions _options;

    public RefreshTokenRepository(IDatabase redis, RefreshTokenSerializer serializer, RefreshTokenRepositoryOptions options)
    {
        _redis = redis;
        _serializer = serializer;
        _options = options;
    }

    public async Task Add(UserId userId, RefreshToken token)
    {
        ValidRefreshTokenCollection tokens = await Get(userId);
        
        tokens.Add(token);

        await Set(tokens, userId);
    }

    public async Task StrictDelete(UserId userId, RefreshToken token)
    {
        ValidRefreshTokenCollection tokens = await Get(userId);

        tokens.StrictDelete(token);

        await Set(tokens, userId);
    }

    public async Task EnsureDeleted(UserId userId, RefreshToken token)
    {
        ValidRefreshTokenCollection tokens = await Get(userId);
        
        tokens.EnsureDeleted(token);

        await Set(tokens, userId);
    }

    public async Task DeleteAllForUser(UserId userId)
    {
        await _redis.KeyDeleteAsync(BuildKey(userId));
    }

    private async Task<ValidRefreshTokenCollection> Get(UserId userId)
    {
        RedisValue redisValue = await _redis.StringGetAsync(BuildKey(userId));

        ValidRefreshTokenCollection refreshTokens;
        if (redisValue.HasValue)
        {
            refreshTokens = _serializer.DeserializeCollection(redisValue.ToString(), _options.Expire);
        }
        else
        {
            refreshTokens = new ValidRefreshTokenCollection(new List<TimestampRefreshToken>(), _options.Expire);
        }

        return refreshTokens;
    }

    private async Task Set(ValidRefreshTokenCollection refreshTokens, UserId userId)
    {
        await _redis.StringSetAsync(BuildKey(userId), _serializer.SerializeCollection(refreshTokens));
    }

    private string BuildKey(UserId userId)
    {
        return $"{userId.ToString()}-refresh-tokens";
    }
}