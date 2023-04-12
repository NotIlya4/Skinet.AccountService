using Domain.Primitives;
using Infrastructure.RefreshTokenSystem;

namespace UnitTests.ValidRefreshTokenCollectionTests;

public class ValidRefreshTokenCollectionTests
{
    public TimeSpan Expire { get; } = TimeSpan.FromMinutes(15);
    public RefreshToken RefreshToken1 { get; } = new RefreshToken(new Guid("43af8d5b-b40c-4418-9a7c-f09439a10728"));
    public RefreshToken RefreshToken2 { get; } = new RefreshToken(new Guid("cdf123cc-427a-448b-94bd-c22f06a0c8f5"));
    public RefreshToken RefreshToken3 { get; } = new RefreshToken(new Guid("f19ad07e-5459-4cc1-8bfe-d21a4c36bd06"));
    public RefreshToken RefreshToken4 { get; } = new RefreshToken(new Guid("396d7559-9d7a-448d-bf71-44fd103cba84"));
    public RefreshToken RefreshToken5 { get; } = new RefreshToken(new Guid("0a0517ef-6bae-4444-86d7-5891e0af3ff3"));

    [Fact]
    public void Constructor_RandomValid_RemainOnlyValid()
    {
        List<TimestampRefreshToken> refreshTokens = new()
        {
            new TimestampRefreshToken() { RefreshToken = RefreshToken3, Issued = DateTime.UtcNow.AddMinutes(-30) },
            new TimestampRefreshToken() { RefreshToken = RefreshToken4, Issued = DateTime.UtcNow.AddMinutes(-23) },
            new TimestampRefreshToken() { RefreshToken = RefreshToken1, Issued = DateTime.UtcNow.AddMinutes(-17) },
            new TimestampRefreshToken() { RefreshToken = RefreshToken2, Issued = DateTime.UtcNow.AddMinutes(-3) },
            new TimestampRefreshToken() { RefreshToken = RefreshToken5, Issued = DateTime.UtcNow.AddMinutes(0) },
        };
        List<RefreshToken> expect = new()
        {
            RefreshToken2,
            RefreshToken5
        };

        List<RefreshToken> tokensAfterConstructor = ApplyConstructor(refreshTokens);
        
        Assert.Equal(expect, tokensAfterConstructor);
    }

    [Fact]
    public void Constructor_AllValid_RemainAll()
    {
        List<TimestampRefreshToken> refreshTokens = new()
        {
            new TimestampRefreshToken() { RefreshToken = RefreshToken3, Issued = DateTime.UtcNow.AddMinutes(-5) },
            new TimestampRefreshToken() { RefreshToken = RefreshToken4, Issued = DateTime.UtcNow.AddMinutes(-3) },
            new TimestampRefreshToken() { RefreshToken = RefreshToken1, Issued = DateTime.UtcNow.AddMinutes(-3) },
            new TimestampRefreshToken() { RefreshToken = RefreshToken2, Issued = DateTime.UtcNow.AddMinutes(-2) }
        };
        List<RefreshToken> expect = new()
        {
            RefreshToken3,
            RefreshToken4,
            RefreshToken1,
            RefreshToken2
        };

        List<RefreshToken> tokensAfterConstructor = ApplyConstructor(refreshTokens);
        
        Assert.Equal(expect, tokensAfterConstructor);
    }

    [Fact]
    public void Constructor_AllNotValid_EmtpyList()
    {
        List<TimestampRefreshToken> refreshTokens = new()
        {
            new TimestampRefreshToken() { RefreshToken = RefreshToken3, Issued = DateTime.UtcNow.AddMinutes(-50) },
            new TimestampRefreshToken() { RefreshToken = RefreshToken4, Issued = DateTime.UtcNow.AddMinutes(-45) },
            new TimestampRefreshToken() { RefreshToken = RefreshToken1, Issued = DateTime.UtcNow.AddMinutes(-41) },
            new TimestampRefreshToken() { RefreshToken = RefreshToken2, Issued = DateTime.UtcNow.AddMinutes(-37) },
            new TimestampRefreshToken() { RefreshToken = RefreshToken5, Issued = DateTime.UtcNow.AddMinutes(-33) },
        };
        List<RefreshToken> expect = new();

        List<RefreshToken> tokensAfterConstructor = ApplyConstructor(refreshTokens);
        
        Assert.Equal(expect, tokensAfterConstructor);
    }

    public List<RefreshToken> ApplyConstructor(List<TimestampRefreshToken> tokens)
    {
        ValidRefreshTokenCollection collection = new(tokens, Expire);
        return collection.ToList().Select(t => t.RefreshToken).ToList();
    }
}