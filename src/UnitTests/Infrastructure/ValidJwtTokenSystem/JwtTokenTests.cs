using Infrastructure.JwtTokenHelper;

namespace UnitTests.Infrastructure;

public class JwtTokenTests
{
    private readonly string _rawJwtToken = "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJzdWIiOiIwODJhOWYwYi0zMDNhLTQ5ZDctYmU2OC1lOTQ0ZjA4YTNmMTkiLCJuYW1lIjoiSm9obiBEb2UiLCJpYXQiOjE1MTYyMzkwMjIsImV4cCI6MTUxNjIzOTEyMn0.XnO7THg97M5oncKyzWdoEz27Rs4ZdNSiZ8PHozGrbXw";
    private readonly UserId _userId = new UserId("082a9f0b-303a-49d7-be68-e944f08a3f19");
    private readonly DateTime _issuedAt = DateTimeOffset.FromUnixTimeSeconds(1516239022).DateTime;
    private readonly DateTime _expires = DateTimeOffset.FromUnixTimeSeconds(1516239122).DateTime;
    private readonly JwtToken _jwtToken;

    public JwtTokenTests()
    {
        _jwtToken = new JwtToken(_rawJwtToken);
    }

    [Fact]
    public void CreateJwtTokenWithSpecifiedJwtToken_ParseValues()
    {
        Assert.Equal(_rawJwtToken, _jwtToken.Raw);
        Assert.Equal(_userId, _jwtToken.UserId);
        Assert.Equal(_issuedAt, _jwtToken.IssuedAt);
        Assert.Equal(_expires, _jwtToken.ValidTo);
    }
}