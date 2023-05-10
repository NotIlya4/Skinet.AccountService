namespace Infrastructure.JwtTokenHelper;

public interface IJwtTokenHelper
{
    public ValidJwtToken Validate(string rawJwtToken);
    public ValidJwtToken Validate(JwtToken jwtToken);
    public JwtToken Create(UserId userId);
}