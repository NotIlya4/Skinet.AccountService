namespace Infrastructure.JwtTokenManager;

public interface IJwtTokenManager
{
    public string CreateJwtToken(Guid userId);
    public void Validate(string token);
}