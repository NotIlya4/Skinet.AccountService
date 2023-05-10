using Infrastructure.ValidJwtTokenSystem.Models;

namespace Infrastructure.ValidJwtTokenSystem;

public class ValidJwtTokenFactory : IValidJwtTokenFactory
{
    private readonly IJwtTokenValidator _validator;
    
    public ValidJwtTokenFactory(IJwtTokenValidator validator)
    {
        _validator = validator;
    }

    public ValidJwtToken Create(string rawJwtToken)
    {
        return new ValidJwtToken(_validator, rawJwtToken);
    }

    public ValidJwtToken Create(JwtToken jwtToken)
    {
        return new ValidJwtToken(_validator, jwtToken);
    }
}