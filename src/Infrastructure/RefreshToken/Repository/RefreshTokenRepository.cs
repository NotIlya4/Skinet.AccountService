using System.Text.Json;
using Domain.Primitives;
using Newtonsoft.Json.Linq;
using StackExchange.Redis;

namespace Infrastructure.RefreshToken;

public class RefreshTokenRepository : IRefreshTokenRepository
{
    private readonly IDatabase _redis;
    private readonly TokenRepositoryOptions _options;

    public RefreshTokenRepository(IDatabase redis, TokenRepositoryOptions options)
    {
        _redis = redis;
        _options = options;
    }

    public async Task Add(UserId userId, Guid token)
    {
        ValidRefreshTokenCollection tokens = await Get(userId);
        
        tokens.Add(token);

        await Set(tokens, userId);
    }

    public async Task StrictDelete(UserId userId, Guid token)
    {
        ValidRefreshTokenCollection tokens = await Get(userId);

        tokens.StrictDelete(token);

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
            refreshTokens = DeserializeRefreshTokens(redisValue.ToString());
        }
        else
        {
            refreshTokens = new ValidRefreshTokenCollection(new List<TimestampRefreshToken>(), _options.JwtRefreshTokenExpireTime);
        }

        return refreshTokens;
    }

    private async Task Set(ValidRefreshTokenCollection refreshTokens, UserId userId)
    {
        await _redis.StringSetAsync(BuildKey(userId), SerializeRefreshTokens(refreshTokens));
    }

    private string SerializeRefreshTokens(ValidRefreshTokenCollection refreshToken)
    {
        return new JArray(refreshToken.ToList().Select(JObject.FromObject).ToList()).ToString();
    }

    private ValidRefreshTokenCollection DeserializeRefreshTokens(string rawRefreshToken)
    {
        return new ValidRefreshTokenCollection(
            JArray.Parse(rawRefreshToken).ToObject<List<TimestampRefreshToken>>()!,
            _options.JwtRefreshTokenExpireTime);
    }

    private string BuildKey(UserId userId)
    {
        return $"{userId.ToString()}-refresh-tokens";
    }
}