using System.IdentityModel.Tokens.Jwt;
using System.Text;
using Infrastructure.JwtTokenSystem.Manager;
using Microsoft.IdentityModel.Tokens;

namespace UnitTests.Infrastructure;

public class JwtTokenManagerTests
{
    public JwtTokenManager Manager { get; }
    public JwtTokenManagerOptions Options { get; }
    public Guid UserId { get; } = new Guid("a6e96499-c80a-474d-a5d4-0ad065eb19c0");
    public string RawToken { get; }
    public JwtSecurityToken Token { get; }
    public string RawExpiredToken { get; }
    public JwtSecurityToken ExpiredToken { get; }
    
    public JwtTokenManagerTests()
    {
        Options = new JwtTokenManagerOptions(
            issuer: "AccountService",
            audience: "Api",
            secret: new SymmetricSecurityKey(Encoding.UTF8.GetBytes("1tsJusT@S@mpleP@ssword!")),
            expire: TimeSpan.FromMinutes(15));
        
        Manager = new JwtTokenManager(Options);
        RawToken = Manager.CreateJwtToken(UserId);
        Token = new JwtSecurityToken(RawToken);
        ExpiredToken = AlterExpires(Token, DateTime.UtcNow.AddMinutes(-30), DateTime.UtcNow.AddMinutes(-15));
        RawExpiredToken = Serialize(ExpiredToken);
    }

    [Fact]
    public void CreateJwtToken_PassUserId_JwtTokenWithThatUserId()
    {
        Guid userId = new Guid(Token.Subject);
        
        Assert.Equal(UserId, userId);
    }
    
    [Fact]
    public void CreateJwtToken_ExpireSetAfter15Mins_ValidToAfter15Mins()
    {
        DateTime expectValidTo = DateTime.UtcNow.AddMinutes(15);

        DateTime validTo = Token.ValidTo;

        Assert.True(IsNearlyEqualTime(expectValidTo, validTo));
    }

    [Fact]
    public void CreateJwtToken_CreatedTime_ValidFromAndIssuedAtNow()
    {
        DateTime issuedTime = DateTime.UtcNow;

        DateTime validFrom = Token.ValidFrom;
        DateTime issued = Token.IssuedAt;
        
        Assert.True(IsNearlyEqualTime(issuedTime, validFrom));
        Assert.True(IsNearlyEqualTime(issuedTime, issued));
    }

    [Fact]
    public void ValidateAndExtractUserId_ExpiredToken_ThrowException()
    {
        Assert.Throws<SecurityTokenExpiredException>(() => { Manager.ValidateAndExtractUserId(RawExpiredToken); });
    }

    [Fact]
    public void ValidateAndExtractUserId_ValidToken_UserId()
    {
        Guid result = Manager.ValidateAndExtractUserId(RawToken);
        
        Assert.Equal(UserId, result);
    }

    [Fact]
    public void ExtractUserId_InvalidToken_UserId()
    {
        Guid result = Manager.ExtractUserId(RawToken);
        
        Assert.Equal(UserId, result);
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