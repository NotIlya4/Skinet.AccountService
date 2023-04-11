namespace Infrastructure.JwtToken;

public class InvalidJwtTokenException : Exception
{
    public InvalidJwtTokenException(string msg) : base(msg)
    {
        
    }
}