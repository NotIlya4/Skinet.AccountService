namespace Infrastructure.JwtTokenHelper;

public interface IJwtTokenValidator
{
    public void Validate(string jwtToken);
}