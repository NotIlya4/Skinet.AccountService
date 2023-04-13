namespace Infrastructure.JwtTokenManager;

public class InvalidJwtTokenException : Exception
{
    public InvalidJwtTokenException(string msg) : base(msg)
    {
        
    }
}