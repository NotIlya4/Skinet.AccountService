namespace Domain.Primitives;

public readonly struct RefreshToken
{
    private readonly Guid _refreshToken;

    public RefreshToken(string id) : this(new Guid(id)) { }
    
    public RefreshToken(Guid refreshToken)
    {
        _refreshToken = refreshToken;
    }

    public bool Equals(RefreshToken other)
    {
        return _refreshToken.Equals(other._refreshToken);
    }

    public override bool Equals(object? obj)
    {
        if (obj is RefreshToken objUserId)
        {
            return Equals(objUserId);
        }

        return false;
    }

    public static bool operator ==(RefreshToken left, RefreshToken right)
    {
        return left.Equals(right);
    }

    public static bool operator !=(RefreshToken left, RefreshToken right)
    {
        return !(left == right);
    }

    public override int GetHashCode()
    {
        return _refreshToken.GetHashCode();
    }

    public override string ToString()
    {
        return _refreshToken.ToString();
    }
}