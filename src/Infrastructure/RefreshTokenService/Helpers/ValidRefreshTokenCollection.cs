using Infrastructure.RefreshTokenService.Exceptions;
using Infrastructure.RefreshTokenService.Models;

namespace Infrastructure.RefreshTokenService.Helpers;

public record ValidRefreshTokenCollection
{
    private List<TimestampRefreshToken> RefreshTokens { get; }
    private TimeSpan ExpireIn { get; }
    public List<RefreshToken> Tokens => RefreshTokens.Select(t => t.RefreshToken).ToList();
    
    public ValidRefreshTokenCollection(List<TimestampRefreshToken> refreshTokens, TimeSpan expireIn)
    {
        RefreshTokens = refreshTokens;
        ExpireIn = expireIn;
        
        CleanExpireTokens();
    }

    public void Add(RefreshToken token)
    {
        RefreshTokens.Add(new TimestampRefreshToken(issued: DateTime.UtcNow, refreshToken: token));
    }

    public void StrictDelete(RefreshToken token)
    {
        TimestampRefreshToken? tokenToDelete = RefreshTokens.FirstOrDefault(t => t.RefreshToken == token);

        if (tokenToDelete is null || !RefreshTokens.Remove(tokenToDelete))
        {
            throw new RefreshTokenNotFoundException(token);
        }
    }

    public void EnsureDeleted(RefreshToken token)
    {
        TimestampRefreshToken? tokenToDelete = RefreshTokens.FirstOrDefault(t => t.RefreshToken == token);

        if (tokenToDelete is not null)
        {
            RefreshTokens.Remove(tokenToDelete);
        }
    }

    public bool Contains(RefreshToken token)
    {
        return RefreshTokens.FirstOrDefault(t => t.RefreshToken == token) is not null;
    }

    public List<TimestampRefreshToken> ToList()
    {
        return RefreshTokens.ToList();
    }

    public virtual bool Equals(ValidRefreshTokenCollection? other)
    {
        if (other is null)
        {
            return false;
        }

        return ExpireIn == other.ExpireIn && RefreshTokens.SequenceEqual(other.RefreshTokens);
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(RefreshTokens, ExpireIn);
    }

    private void CleanExpireTokens()
    {
        List<DateTime> dates = RefreshTokens.Select(t => t.Issued).ToList();
        RefreshTokens.RemoveRange(0, TimespanBinarySearch.DeleteCount(dates, ExpireIn));
    }
}