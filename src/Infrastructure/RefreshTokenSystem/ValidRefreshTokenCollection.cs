using Infrastructure.RefreshTokenSystem.Repository;

namespace Infrastructure.RefreshTokenSystem;

public readonly record struct ValidRefreshTokenCollection
{
    private readonly List<TimestampRefreshToken> _refreshTokens;
    private readonly TimeSpan _expireIn;
    
    public ValidRefreshTokenCollection(List<TimestampRefreshToken> refreshTokens, TimeSpan expireIn)
    {
        _refreshTokens = refreshTokens;
        _expireIn = expireIn;
        
        CleanExpireTokens();
    }

    public void Add(Guid token)
    {
        _refreshTokens.Add(new TimestampRefreshToken() {Issued = DateTime.UtcNow, RefreshToken = token});
    }

    public void StrictDelete(Guid token)
    {
        TimestampRefreshToken? tokenToDelete = _refreshTokens.FirstOrDefault(t => t.RefreshToken == token);

        if (tokenToDelete is null || !_refreshTokens.Remove(tokenToDelete))
        {
            throw new RefreshTokenNotFoundException(token);
        }
    }

    public void EnsureDeleted(Guid token)
    {
        TimestampRefreshToken? tokenToDelete = _refreshTokens.FirstOrDefault(t => t.RefreshToken == token);

        if (tokenToDelete is not null)
        {
            _refreshTokens.Remove(tokenToDelete);
        }
    }

    public List<TimestampRefreshToken> ToList()
    {
        return _refreshTokens;
    }

    private void CleanExpireTokens()
    {
        int left = -1;
        int right = _refreshTokens.Count - 1;

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
            
            TimeSpan issueDelta = DateTime.UtcNow - _refreshTokens[mid].Issued;

            bool isGoingToBeDeleted = issueDelta >= _expireIn;

            if (isGoingToBeDeleted)
            {
                left = mid;
            }
            else
            {
                right = Math.Clamp(mid - 1, left, right);
            }
        }

        _refreshTokens.RemoveRange(0, left + 1);
    }
}