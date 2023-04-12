namespace Infrastructure.RefreshToken;

public class RefreshTokenNotFoundException : Exception
{
    public RefreshTokenNotFoundException(Guid token) : base($"{token.ToString()} not found")
    {
        
    }
}