using Infrastructure.RefreshTokenService.Helpers;
using Infrastructure.RefreshTokenService.Models;
using StackExchange.Redis;

namespace Infrastructure.RefreshTokenService;

public class RefreshTokenService : IRefreshTokenService
{
    private readonly IDatabase _redis;
    private readonly RefreshTokenSerializer _serializer;
    private readonly RefreshTokenRepositoryOptions _options;

    public RefreshTokenService(IDatabase redis, RefreshTokenSerializer serializer, RefreshTokenRepositoryOptions options)
    {
        _redis = redis;
        _serializer = serializer;
        _options = options;
    }

    public async Task<RefreshToken> CreateNew(UserId userId)
    {
        ValidRefreshTokenCollection tokens = await Get(userId);
        var token = new RefreshToken(Guid.NewGuid());
        tokens.Add(token);
        await Set(tokens, userId);

        return token;
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

    public async Task<bool> Contains(UserId userId, RefreshToken token)
    {
        ValidRefreshTokenCollection tokens = await Get(userId);
        return tokens.Contains(token);
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
        await _redis.StringSetAsync(BuildKey(userId), _serializer.SerializeCollection(refreshTokens), _options.Expire);
    }

    private string BuildKey(UserId userId)
    {
        return $"{userId.Value.ToString()}-refresh-tokens";
    }
}