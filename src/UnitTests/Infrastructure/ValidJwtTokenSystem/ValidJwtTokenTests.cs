using Infrastructure.JwtTokenHelper;
using Moq;

namespace UnitTests.Infrastructure;

public class ValidJwtTokenTests
{
    [Fact]
    public void Constructor_PassMock_RunMethodWithPassedToken()
    {
        string jwtToken = "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJzdWIiOiI4NDMwZWYwZi05NWQxLTQ3ZGItOThhZS1iMWZkM2FhYjIzMjUiLCJuYW1lIjoiSm9obiBEb2UiLCJpYXQiOjE1MTYyMzkwMjJ9.ooCR2pK3ySvKFuKW5knCR0dZF80GZLI4jaIhrROqWYE";
        var validator = new Mock<IJwtTokenValidator>();

        new ValidJwtToken(validator.Object, jwtToken);
        
        validator.Verify(v => v.Validate(jwtToken), Times.Once());
    }
}