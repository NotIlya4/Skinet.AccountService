namespace Infrastructure.ValidJwtTokenSystem;

public interface IJwtTokenValidator
{
    public void Validate(string jwtToken);
}