using Infrastructure.ValidJwtTokenSystem.Models;

namespace Infrastructure.ValidJwtTokenSystem;

public interface IValidJwtTokenFactory
{
    public ValidJwtToken Create(string rawJwtToken);
    public ValidJwtToken Create(JwtToken jwtToken);
}