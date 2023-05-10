using Infrastructure.RefreshTokenService.Models;

namespace Infrastructure.RefreshTokenService.Exceptions;

public class RefreshTokenNotFoundException : Exception
{
    public RefreshTokenNotFoundException(RefreshToken token) : base($"{token.Value.ToString()} not found")
    {
        
    }
}