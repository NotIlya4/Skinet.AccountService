namespace Infrastructure;

public readonly struct UserId
{
    private readonly Guid _id;

    public UserId(string id) : this(new Guid(id)) { }
    
    public UserId(Guid id)
    {
        _id = id;
    }

    public bool Equals(UserId other)
    {
        return _id.Equals(other._id);
    }

    public override bool Equals(object? obj)
    {
        if (obj is UserId objUserId)
        {
            return Equals(objUserId);
        }

        return false;
    }

    public static bool operator ==(UserId left, UserId right)
    {
        return left.Equals(right);
    }

    public static bool operator !=(UserId left, UserId right)
    {
        return !(left == right);
    }

    public override int GetHashCode()
    {
        return _id.GetHashCode();
    }

    public override string ToString()
    {
        return _id.ToString();
    }
}