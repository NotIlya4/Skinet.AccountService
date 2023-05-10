namespace Infrastructure.ValidJwtTokenSystem.Models;

public record ValidJwtToken
{
    public JwtToken Token { get; }
    
    public ValidJwtToken(IJwtTokenValidator validator, string rawJwtToken) : this(validator, new JwtToken(rawJwtToken))
    {
        
    }

    public ValidJwtToken(IJwtTokenValidator validator, JwtToken jwtToken)
    {
        validator.Validate(jwtToken.Raw);

        Token = jwtToken;
    }

    public override string ToString()
    {
        return Token.Raw;
    }
}