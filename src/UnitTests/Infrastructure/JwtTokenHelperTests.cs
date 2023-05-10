using System.IdentityModel.Tokens.Jwt;
using System.Text;
using Infrastructure.JwtTokenHelper;
using Microsoft.IdentityModel.Tokens;

namespace UnitTests.Infrastructure;

public class JwtTokenHelperTests
{
    private readonly IJwtTokenHelper _helper;
    private readonly JwtTokenHelperOptions _options;
    private readonly UserId _userId = new("a6e96499-c80a-474d-a5d4-0ad065eb19c0");
    private readonly string _rawToken;
    private readonly JwtToken _token;
    private readonly JwtToken _expiredToken;
    private readonly SymmetricSecurityKey _secret;
    
    public JwtTokenHelperTests()
    {
        _options = new JwtTokenHelperOptions(
            issuer: "AccountService",
            audience: "Api",
            secret: "1tsJusT@S@mpleP@ssword!",
            expire: TimeSpan.FromMinutes(15));

        _secret = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_options.Secret));
        _helper = new JwtTokenHelper(_options);
        _token = _helper.Create(_userId);
        _rawToken = _token.Raw;
        _expiredToken = AlterExpires(_token, DateTime.UtcNow.AddMinutes(-30), DateTime.UtcNow.AddMinutes(-15));
    }

    [Fact]
    public void CreateJwtToken_PassUserId_JwtTokenWithThatUserId()
    {
        Assert.Equal(_userId, _token.UserId);
    }
    
    [Fact]
    public void CreateJwtToken_CreateToken_TokenMustBeValidFor15Mins()
    {
        DateTime expectValidTo = DateTime.UtcNow.AddMinutes(15);

        DateTime validTo = _token.ValidTo;

        Assert.True(IsNearlyEqualTime(expectValidTo, validTo));
    }

    [Fact]
    public void CreateJwtToken_UserId_TokenWithThatUserId()
    {
        Assert.Equal(_userId, _token.UserId);
    }

    [Fact]
    public void Validate_ExpiredToken_Throw()
    {
        Assert.Throws<SecurityTokenExpiredException>(() => { _helper.Validate(_expiredToken); });
    }

    private JwtToken AlterExpires(JwtToken token, DateTime notBefore, DateTime expires)
    {
        var jwtSecurityToken = new JwtSecurityToken(
            issuer: token.JwtSecurityToken.Issuer,
            audience: _options.Audience,
            claims: token.JwtSecurityToken.Claims,
            notBefore: notBefore,
            expires: expires,
            signingCredentials: new SigningCredentials(_secret, SecurityAlgorithms.HmacSha256));

        return new JwtToken(Serialize(jwtSecurityToken));
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