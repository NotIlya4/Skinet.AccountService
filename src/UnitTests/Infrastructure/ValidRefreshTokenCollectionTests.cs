using Infrastructure.RefreshTokenService.Exceptions;
using Infrastructure.RefreshTokenService.Helpers;
using Infrastructure.RefreshTokenService.Models;

namespace UnitTests.Infrastructure;

public class ValidRefreshTokenCollectionTests
{
    private readonly TimeSpan _expire = TimeSpan.FromMinutes(15);
    private readonly RefreshToken _refreshToken1 = new("43af8d5b-b40c-4418-9a7c-f09439a10728");
    private readonly RefreshToken _refreshToken2 = new("cdf123cc-427a-448b-94bd-c22f06a0c8f5");
    private readonly RefreshToken _refreshToken3 = new ("f19ad07e-5459-4cc1-8bfe-d21a4c36bd06");
    private readonly RefreshToken _refreshToken4 = new("396d7559-9d7a-448d-bf71-44fd103cba84");
    private readonly RefreshToken _refreshToken5 = new("0a0517ef-6bae-4444-86d7-5891e0af3ff3");
    private readonly ValidRefreshTokenCollection _collection;

    public ValidRefreshTokenCollectionTests()
    {
        _collection = new ValidRefreshTokenCollection(new List<TimestampRefreshToken>
        {
            new(refreshToken: _refreshToken4, issued: DateTime.UtcNow.AddMinutes(-30)),
            new(refreshToken: _refreshToken5, issued: DateTime.UtcNow.AddMinutes(-29)),
            new(refreshToken: _refreshToken1, issued: DateTime.UtcNow.AddMinutes(-10)),
            new(refreshToken: _refreshToken2, issued: DateTime.UtcNow.AddMinutes(-9)),
            new(refreshToken: _refreshToken3, issued: DateTime.UtcNow.AddMinutes(-8)),
        }, _expire);
    }

    [Fact]
    public void Constructor_SomeTokensValidSomeExpired_RemainOnlyValid()
    {
        List<RefreshToken> expect = new()
        {
            _refreshToken1,
            _refreshToken2,
            _refreshToken3
        };

        Assert.Equal(expect, _collection.Tokens);
    }

    [Fact]
    public void StrictDelete_DeleteTwoTimesSameToken_ThrowOnSecondDelete()
    {
        _collection.StrictDelete(_refreshToken1);
        Assert.Throws<RefreshTokenNotFoundException>(() => _collection.StrictDelete(_refreshToken1));
    }

    [Fact]
    public void StrictDelete_DeleteTokenThatNotInCollection_Throw()
    {
        Assert.Throws<RefreshTokenNotFoundException>(() => _collection.StrictDelete(_refreshToken4));
    }
    
    [Fact]
    public void EnsureDeleted_DeleteToken_NotContains()
    {
        _collection.EnsureDeleted(_refreshToken1);
        
        Assert.False(_collection.Contains(_refreshToken1));
    }

    [Fact]
    public void EnsureDeleted_DeleteTwoTimesSameToken_NotThrow()
    {
        _collection.EnsureDeleted(_refreshToken1);
        _collection.EnsureDeleted(_refreshToken1);
        
        Assert.False(_collection.Contains(_refreshToken1));
    }

    [Fact]
    public void EnsureDeleted_DeleteTokenThatNotInCollection_NotThrow()
    {
        _collection.EnsureDeleted(_refreshToken4);
    }

    [Fact]
    public void Contains_CollectionDoesntContainToken_False()
    {
        Assert.False(_collection.Contains(_refreshToken4));
    }
    
    [Fact]
    public void Contains_CollectionContainsToken_True()
    {
        Assert.True(_collection.Contains(_refreshToken3));
    }
}