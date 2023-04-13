using Domain.Primitives;

namespace Infrastructure.JwtTokenManager;

public interface IJwtTokenManager
{
    public string CreateJwtToken(UserId userId);
    public UserId ValidateAndGetUserId(string token);
}