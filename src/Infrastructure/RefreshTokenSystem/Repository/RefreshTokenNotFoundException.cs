using Domain.Primitives;

namespace Infrastructure.RefreshTokenSystem.Repository;

public class RefreshTokenNotFoundException : Exception
{
    public RefreshTokenNotFoundException(RefreshToken token) : base($"{token.ToString()} not found")
    {
        
    }
}