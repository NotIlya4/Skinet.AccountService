using Infrastructure.RefreshTokenService.Exceptions;
using Infrastructure.RefreshTokenService.Models;

namespace Infrastructure.RefreshTokenService.Helpers;

public record ValidRefreshTokenCollection
{
    private List<TimestampRefreshToken> RefreshTokens { get; }
    private TimeSpan ExpireIn { get; }
    
    public ValidRefreshTokenCollection(List<TimestampRefreshToken> refreshTokens, TimeSpan expireIn)
    {
        RefreshTokens = refreshTokens;
        ExpireIn = expireIn;
        
        CleanExpireTokens();
    }

    public void Add(Guid token)
    {
        RefreshTokens.Add(new TimestampRefreshToken(issued: DateTime.UtcNow, refreshToken: token));
    }

    public void StrictDelete(Guid token)
    {
        TimestampRefreshToken? tokenToDelete = RefreshTokens.FirstOrDefault(t => t.RefreshToken == token);

        if (tokenToDelete is null || !RefreshTokens.Remove(tokenToDelete))
        {
            throw new RefreshTokenNotFoundException(token);
        }
    }

    public void EnsureDeleted(Guid token)
    {
        TimestampRefreshToken? tokenToDelete = RefreshTokens.FirstOrDefault(t => t.RefreshToken == token);

        if (tokenToDelete is not null)
        {
            RefreshTokens.Remove(tokenToDelete);
        }
    }

    public List<TimestampRefreshToken> ToList()
    {
        return RefreshTokens.ToList();
    }

    private void CleanExpireTokens()
    {
        int left = -1;
        int right = RefreshTokens.Count - 1;

        while (left < right)
        {
            int mid;
            if (right - left == 1)
            {
                mid = right;
            }
            else
            {
                mid = (left + right) / 2;
            }
            
            TimeSpan issueDelta = DateTime.UtcNow - RefreshTokens[mid].Issued;

            bool isGoingToBeDeleted = issueDelta >= ExpireIn;

            if (isGoingToBeDeleted)
            {
                left = mid;
            }
            else
            {
                right = Math.Clamp(mid - 1, left, right);
            }
        }

        RefreshTokens.RemoveRange(0, left + 1);
    }
}