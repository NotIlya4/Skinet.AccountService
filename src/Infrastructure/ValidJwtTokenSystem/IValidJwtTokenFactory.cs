namespace Infrastructure.JwtTokenHelper;

public interface IValidJwtTokenFactory
{
    public ValidJwtToken Create(string rawJwtToken);
    public ValidJwtToken Create(JwtToken jwtToken);
}