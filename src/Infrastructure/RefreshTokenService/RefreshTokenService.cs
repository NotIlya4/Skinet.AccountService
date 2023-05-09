using Infrastructure.RefreshTokenRepository.Helpers;
using Infrastructure.RefreshTokenRepository.Models;
using StackExchange.Redis;

namespace Infrastructure.RefreshTokenRepository;

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

    public async Task Add(Guid userId, Guid token)
    {
        ValidRefreshTokenCollection tokens = await Get(userId);
        
        tokens.Add(token);

        await Set(tokens, userId);
    }

    public async Task StrictDelete(Guid userId, Guid token)
    {
        ValidRefreshTokenCollection tokens = await Get(userId);

        tokens.StrictDelete(token);

        await Set(tokens, userId);
    }

    public async Task EnsureDeleted(Guid userId, Guid token)
    {
        ValidRefreshTokenCollection tokens = await Get(userId);
        
        tokens.EnsureDeleted(token);

        await Set(tokens, userId);
    }

    public async Task DeleteAllForUser(Guid userId)
    {
        await _redis.KeyDeleteAsync(BuildKey(userId));
    }

    private async Task<ValidRefreshTokenCollection> Get(Guid userId)
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

    private async Task Set(ValidRefreshTokenCollection refreshTokens, Guid userId)
    {
        await _redis.StringSetAsync(BuildKey(userId), _serializer.SerializeCollection(refreshTokens), _options.Expire);
    }

    private string BuildKey(Guid userId)
    {
        return $"{userId.ToString()}-refresh-tokens";
    }
}