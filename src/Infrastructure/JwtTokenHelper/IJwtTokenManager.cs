using Domain.Primitives;

namespace Infrastructure.JwtToken;

public interface IJwtTokenManager
{
    public string CreateJwtToken(UserId userId);
    public UserId ValidateAndGetUserId(string token);
}