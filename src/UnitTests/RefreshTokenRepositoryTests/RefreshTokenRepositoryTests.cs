using Domain.Primitives;
using Infrastructure;
using Infrastructure.RefreshTokenSystem;
using Infrastructure.RefreshTokenSystem.Repository;
using Newtonsoft.Json.Linq;
using StackExchange.Redis;

namespace UnitTests.RefreshTokenRepositoryTests;

public class RefreshTokenRepositoryTests : IDisposable
{
    public RefreshTokenRepository Repository { get; }
    public IDatabase Redis { get; }
    public RefreshTokenSerializer Serializer { get; }
    public UserId UserWithRandomTokens { get; } = new UserId("5a1e1ce5-7734-4fdb-a721-2cc70f316c81");
    public TimestampRefreshToken ExpiredToken1 { get; }
    public TimestampRefreshToken ExpiredToken2 { get; }
    public TimestampRefreshToken ValidToken1 { get; }
    public TimestampRefreshToken ValidToken2 { get; }
    
    public RefreshTokenRepositoryTests()
    {
        ConnectionMultiplexer connectionMultiplexer = ConnectionMultiplexer.Connect("localhost");
        Redis = connectionMultiplexer.GetDatabase(15);
        Redis.Execute("FLUSHDB");
        Serializer = new RefreshTokenSerializer();

        Repository = new RefreshTokenRepository(Redis, Serializer,
            new RefreshTokenRepositoryOptions() { Expire = TimeSpan.FromDays(1) });

        ExpiredToken1 = new TimestampRefreshToken()
            { Issued = DateTime.UtcNow.AddDays(-3), RefreshToken = new RefreshToken(new Guid("7e2311f4-c9ee-40de-b5c4-b4f11977d80f")) };
        ExpiredToken2 = new TimestampRefreshToken()
            { Issued = DateTime.UtcNow.AddDays(-2), RefreshToken = new RefreshToken(new Guid("db3b41d8-7c12-4512-bc41-740d0c77eeb7")) };

        ValidToken1 = new TimestampRefreshToken()
            { Issued = DateTime.UtcNow.AddHours(-5), RefreshToken = new RefreshToken(new Guid("960a12f0-aef1-46d7-8b1f-44bf07bb0d86")) };
        ValidToken2 = new TimestampRefreshToken()
            { Issued = DateTime.UtcNow.AddHours(-2), RefreshToken = new RefreshToken(new Guid("2624e694-a8ed-4f5d-8a34-e92feca21cc7")) };

        List<TimestampRefreshToken> randomTokens = new List<TimestampRefreshToken>()
        {
            ExpiredToken1,
            ExpiredToken2,
            ValidToken1,
            ValidToken2
        };
        
        Redis.StringSet(BuildKey(UserWithRandomTokens), SerializeRefreshTokens(randomTokens));
    }

    [Fact]
    public async Task StrictDelete_SomeTokensExpireAndSomeNot_SuccessfullyDeleteUnExpiredToken()
    {
        await Repository.StrictDelete(UserWithRandomTokens, ValidToken2.RefreshToken);
        await Repository.StrictDelete(UserWithRandomTokens, ValidToken1.RefreshToken);
    }

    [Fact]
    public async Task StrictDelete_DeleteSameValidTokenTwice_ThrowExceptionOnSecondTime()
    {
        await Repository.StrictDelete(UserWithRandomTokens, ValidToken2.RefreshToken);

        await AssertStrictDeleteThrows(UserWithRandomTokens, ValidToken2.RefreshToken);
    }

    [Fact]
    public async Task StrictDelete_DeleteExpiredToken_ThrowNotFoundException()
    {
        await AssertStrictDeleteThrows(UserWithRandomTokens, ExpiredToken1.RefreshToken);
        await AssertStrictDeleteThrows(UserWithRandomTokens, ExpiredToken2.RefreshToken);
    }

    [Fact]
    public async Task DeleteAllForUser_StrictDeleteValidToken_ThrowNotFoundException()
    {
        await Repository.DeleteAllForUser(UserWithRandomTokens);

        await AssertStrictDeleteThrows(UserWithRandomTokens, ValidToken1.RefreshToken);
        await AssertStrictDeleteThrows(UserWithRandomTokens, ValidToken2.RefreshToken);
    }
    
    [Fact]
    public async Task Add_AddToken_SuccessfulStrictDelete()
    {
        await Repository.Add(UserWithRandomTokens, new RefreshToken(new Guid("8df2e98a-55be-48fe-8a0e-fa870aa601e9")));
        await Repository.StrictDelete(UserWithRandomTokens, new RefreshToken(new Guid("8df2e98a-55be-48fe-8a0e-fa870aa601e9")));
    }

    public async Task AssertStrictDeleteThrows(UserId userId, RefreshToken token)
    {
        await Assert.ThrowsAsync<RefreshTokenNotFoundException>(async () =>
        {
            await Repository.StrictDelete(userId, token);
        });
    }
    
    public string SerializeRefreshTokens(List<TimestampRefreshToken> refreshTokens)
    {
        return Serializer.SerializeCollection(refreshTokens);
    }

    private string BuildKey(UserId userId)
    {
        return $"{userId.ToString()}-refresh-tokens";
    }

    public void Dispose()
    {
        Redis.Execute("FLUSHDB");
    }
}