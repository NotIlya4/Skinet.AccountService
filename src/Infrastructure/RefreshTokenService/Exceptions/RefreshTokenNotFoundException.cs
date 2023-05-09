namespace Infrastructure.RefreshTokenService.Exceptions;

public class RefreshTokenNotFoundException : Exception
{
    public RefreshTokenNotFoundException(Guid token) : base($"{token.ToString()} not found")
    {
        
    }
}