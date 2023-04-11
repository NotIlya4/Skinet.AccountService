using Infrastructure;
using Infrastructure.RefreshTokenPersistance;
using StackExchange.Redis;

namespace UnitTests.RefreshTokenRepositoryTests;

public class RefreshTokenRepositoryTests : IDisposable
{
    public RefreshTokenRepository Repository { get; }
    public IDatabase Redis { get; }
    public UserId UserId { get; } = new UserId("4605cd1f-cd30-4480-b788-cbbbe009fd9c");
    public Guid RefreshToken { get; } = new Guid("1d1b1a95-ccf7-47d2-8bce-7a04e5aedf1b");
    
    public RefreshTokenRepositoryTests()
    {
        ConnectionMultiplexer connectionMultiplexer = ConnectionMultiplexer.Connect("localhost");
        Redis = connectionMultiplexer.GetDatabase(15);
        Repository = new RefreshTokenRepository(Redis,
            new TokenRepositoryOptions() { JwtRefreshTokenExpireTime = TimeSpan.FromDays(1) });
    }

    [Fact]
    public async Task Add_AddToken_TokenPersistsInSpecificFormat()
    {
        await Repository.Add(UserId, RefreshToken);

        string key = BuildKey(RefreshToken);
        string value = Redis.StringGet(key).ToString();

        Assert.Equal(value, UserId.ToString());
    }

    [Fact]
    public async Task Add_AddToken_TokenHasRightExpiry()
    {
        await Repository.Add(UserId, RefreshToken);

        string key = BuildKey(RefreshToken);

        TimeSpan redisTimeSpan = Redis.KeyTimeToLive(key)!.Value;
        TimeSpan timeSpan = TimeSpan.FromDays(1) - redisTimeSpan;
        
        Assert.True(timeSpan.TotalSeconds < 10);
    }

    [Fact]
    public async Task PopAssociatedUser_RefreshTokenExists_ReturnAssociatedUserId()
    {
        await Repository.Add(UserId, RefreshToken);

        UserId userId = await Repository.PopAssociatedUser(RefreshToken);
        
        Assert.Equal(userId, UserId);
    }
    
    [Fact]
    public async Task PopAssociatedUser_RefreshTokenExists_DeleteRefreshToken()
    {
        await Repository.Add(UserId, RefreshToken);
        await Repository.PopAssociatedUser(RefreshToken);

        string key = BuildKey(RefreshToken);
        RedisValue redisValue = Redis.StringGet(key);
        
        Assert.False(redisValue.HasValue);
    }
    
    [Fact]
    public async Task PopAssociatedUser_RefreshTokenDoesNotExist_ThrowInvalidRefreshTokenException()
    {
        await Assert.ThrowsAsync<InvalidRefreshTokenException>(async () =>
        {
            await Repository.PopAssociatedUser(RefreshToken);
        });
    }

    private string BuildKey(Guid refreshToken)
    {
        return $"refresh-token-{refreshToken.ToString()}";
    }

    public void Dispose()
    {
        Redis.Execute("FLUSHDB");
    }
}