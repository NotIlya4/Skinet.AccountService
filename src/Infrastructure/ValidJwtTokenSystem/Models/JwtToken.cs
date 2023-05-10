using System.IdentityModel.Tokens.Jwt;

namespace Infrastructure.ValidJwtTokenSystem.Models;

public record JwtToken
{
    public string Raw { get; }
    public UserId UserId { get; }
    public DateTime ValidTo { get; }
    public DateTime IssuedAt { get; }
    public JwtSecurityToken JwtSecurityToken { get; }

    public JwtToken(string token)
    {
        JwtSecurityToken = new JwtSecurityToken(token);
        UserId = new UserId(new Guid(JwtSecurityToken.Subject));
        ValidTo = JwtSecurityToken.ValidTo;
        IssuedAt = JwtSecurityToken.IssuedAt;
        Raw = token;
    }

    public override string ToString()
    {
        return Raw;
    }
}