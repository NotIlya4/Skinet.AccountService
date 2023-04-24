namespace Infrastructure.JwtTokenSystem.Manager;

public interface IJwtTokenManager
{
    public string CreateJwtToken(Guid userId);
    public Guid ValidateAndExtractUserId(string jwtToken);
    public Guid ExtractUserId(string jwtToken);
}