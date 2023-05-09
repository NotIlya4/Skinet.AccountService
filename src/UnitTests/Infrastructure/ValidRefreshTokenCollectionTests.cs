using Infrastructure.RefreshTokenRepository.Exceptions;
using Infrastructure.RefreshTokenRepository.Helpers;
using Infrastructure.RefreshTokenRepository.Models;

namespace UnitTests.Infrastructure;

public class ValidRefreshTokenCollectionTests
{
    public TimeSpan Expire { get; } = TimeSpan.FromMinutes(15);
    public Guid RefreshToken1 { get; } = new("43af8d5b-b40c-4418-9a7c-f09439a10728");
    public Guid RefreshToken2 { get; } = new("cdf123cc-427a-448b-94bd-c22f06a0c8f5");
    public Guid RefreshToken3 { get; } = new ("f19ad07e-5459-4cc1-8bfe-d21a4c36bd06");
    public Guid RefreshToken4 { get; } = new("396d7559-9d7a-448d-bf71-44fd103cba84");
    public Guid RefreshToken5 { get; } = new("0a0517ef-6bae-4444-86d7-5891e0af3ff3");
    public ValidRefreshTokenCollection Collection { get; }

    public ValidRefreshTokenCollectionTests()
    {
        Collection = new ValidRefreshTokenCollection(new()
        {
            new TimestampRefreshToken(refreshToken: RefreshToken1, issued: DateTime.UtcNow.AddMinutes(-10)),
            new TimestampRefreshToken(refreshToken: RefreshToken2, issued: DateTime.UtcNow.AddMinutes(-9)),
            new TimestampRefreshToken(refreshToken: RefreshToken3, issued: DateTime.UtcNow.AddMinutes(-8)),
        }, Expire);
    }

    [Fact]
    public void Constructor_SomeTokensValidSomeExpired_RemainOnlyValid()
    {
        List<Guid> expect = new()
        {
            RefreshToken2,
            RefreshToken5
        };

        List<Guid> result = ApplyCtor(new List<TimestampRefreshToken>()
        {
            new(refreshToken: RefreshToken3, issued: DateTime.UtcNow.AddMinutes(-30)),
            new(refreshToken: RefreshToken4, issued: DateTime.UtcNow.AddMinutes(-23)),
            new(refreshToken: RefreshToken1, issued: DateTime.UtcNow.AddMinutes(-17)),
            new(refreshToken: RefreshToken2, issued: DateTime.UtcNow.AddMinutes(-3)),
            new(refreshToken: RefreshToken5, issued: DateTime.UtcNow.AddMinutes(0)),
        });

        Assert.Equal(expect, result);
    }

    [Fact]
    public void Constructor_AllTokensValid_RemainAll()
    {
        List<Guid> expect = new()
        {
            RefreshToken3,
            RefreshToken4,
            RefreshToken1,
            RefreshToken2
        };

        List<Guid> result = ApplyCtor(new List<TimestampRefreshToken>()
        {
            new(refreshToken: RefreshToken3, issued: DateTime.UtcNow.AddMinutes(-5)),
            new(refreshToken: RefreshToken4, issued: DateTime.UtcNow.AddMinutes(-3)),
            new(refreshToken: RefreshToken1, issued: DateTime.UtcNow.AddMinutes(-3)),
            new(refreshToken: RefreshToken2, issued: DateTime.UtcNow.AddMinutes(-2)),
        });
        
        Assert.Equal(expect, result);
    }

    [Fact]
    public void Constructor_AllNotValid_EmtpyList()
    {
        List<Guid> expect = new();

        List<Guid> result = ApplyCtor(new List<TimestampRefreshToken>
        {
            new(refreshToken: RefreshToken3, issued: DateTime.UtcNow.AddMinutes(-50)),
            new(refreshToken: RefreshToken4, issued: DateTime.UtcNow.AddMinutes(-45)),
            new(refreshToken: RefreshToken1, issued: DateTime.UtcNow.AddMinutes(-41)),
            new(refreshToken: RefreshToken2, issued: DateTime.UtcNow.AddMinutes(-37)),
            new(refreshToken: RefreshToken5, issued: DateTime.UtcNow.AddMinutes(-33)),
        });
        
        Assert.Equal(expect, result);
    }

    [Fact]
    public void StrictDelete_DeleteTwoTimesSameToken_ThrowOnSecondDelete()
    {
        Collection.StrictDelete(RefreshToken1);
        Assert.Throws<RefreshTokenNotFoundException>(() => Collection.StrictDelete(RefreshToken1));
    }

    [Fact]
    public void StrictDelete_DeleteTokenThatNotInCollection_Throw()
    {
        Assert.Throws<RefreshTokenNotFoundException>(() => Collection.StrictDelete(RefreshToken4));
    }

    [Fact]
    public void EnsureDeleted_DeleteTwoTimesSameToken_NotThrow()
    {
        Collection.EnsureDeleted(RefreshToken1);
        Collection.EnsureDeleted(RefreshToken1);
    }

    [Fact]
    public void EnsureDeleted_DeleteTokenThatNotInCollection_NotThrow()
    {
        Collection.EnsureDeleted(RefreshToken4);
    }

    public List<Guid> ApplyCtor(List<TimestampRefreshToken> tokens)
    {
        ValidRefreshTokenCollection collection = new(tokens, Expire);
        return collection.ToList().Select(t => t.RefreshToken).ToList();
    }
}