using System.IdentityModel.Tokens.Jwt;
using System.Text;
using Domain.Primitives;
using Infrastructure.JwtTokenManager;
using Microsoft.IdentityModel.Tokens;

namespace UnitTests.JwtTokenManagerTests;

public class JwtTokenManagerTests
{
    public JwtTokenManager Manager { get; }
    public JwtTokenManagerOptions Options { get; }
    public UserId UserId { get; } = new UserId("a6e96499-c80a-474d-a5d4-0ad065eb19c0");
    public string RawToken { get; }
    public JwtSecurityToken Token { get; }
    
    public JwtTokenManagerTests()
    {
        Options = new JwtTokenManagerOptions()
        {
            Issuer = "AccountService",
            Audience = "Api",
            Secret = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("1tsJusT@S@mpleP@ssword!")),
            Expire = TimeSpan.FromMinutes(15),
        };
        Manager = new JwtTokenManager(Options);
        RawToken = Manager.CreateJwtToken(UserId);
        Token = new JwtSecurityToken(RawToken);
        // IdentityModelEventSource.ShowPII = true;
    }

    [Fact]
    public void CreateJwtToken_ValidToken_JwtTokenContainUserId()
    {
        UserId userId = new UserId(Token.Subject);
        
        Assert.Equal(UserId, userId);
    }
    
    [Fact]
    public void CreateJwtToken_ValidToken_ValidToAfter15Mins()
    {
        DateTime expectValidTo = DateTime.UtcNow.AddMinutes(15);

        DateTime validTo = Token.ValidTo;

        Assert.True(IsNearlyEqualTime(expectValidTo, validTo));
    }

    [Fact]
    public void CreateJwtToken_ValidToken_ValidFromAndIssuedAtNow()
    {
        DateTime issuedTime = DateTime.UtcNow;

        DateTime validFrom = Token.ValidFrom;
        DateTime issued = Token.IssuedAt;
        
        Assert.True(IsNearlyEqualTime(issuedTime, validFrom));
        Assert.True(IsNearlyEqualTime(issuedTime, issued));
    }

    [Fact]
    public void ValidateAndGetUserId_ExpiredToken_ThrowException()
    {
        var expiredToken = AlterExpires(Token, DateTime.UtcNow.AddMinutes(-30), DateTime.UtcNow.AddMinutes(-15));

        Assert.Throws<SecurityTokenExpiredException>(() => { Manager.Validate(Serialize(expiredToken)); });
    }

    private JwtSecurityToken AlterExpires(JwtSecurityToken token, DateTime notBefore, DateTime expires)
    {
        return new JwtSecurityToken(
            issuer: token.Issuer,
            audience: Options.Audience,
            claims: token.Claims,
            notBefore: notBefore,
            expires: expires,
            signingCredentials: new SigningCredentials(Options.Secret, SecurityAlgorithms.HmacSha256));
    }

    private string Serialize(JwtSecurityToken token)
    {
        var handler = new JwtSecurityTokenHandler();
        return handler.WriteToken(token);
    }

    private bool IsNearlyEqualTime(DateTime dateTime1, DateTime dateTime2)
    {
        TimeSpan timeSpan = dateTime1 - dateTime2;
        return timeSpan.TotalSeconds < 10;
    }
}