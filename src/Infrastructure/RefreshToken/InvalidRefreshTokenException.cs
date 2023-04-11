namespace Infrastructure.RefreshTokenPersistance;

public class InvalidRefreshTokenException : Exception
{
    public InvalidRefreshTokenException(string msg) : base(msg)
    {
        
    }
}