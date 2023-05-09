using Domain.Exceptions;
using Domain.Primitives;

namespace UnitTests.Domain;

public class UsernameConstructorTests
{
    [Fact]
    public void OnlyLetters_CreateInstance()
    {
        new Username("a");
    }
    
    [Fact]
    public void OnlyDigits_CreateInstance()
    {
        new Username("1");
    }
    
    [Fact]
    public void NotLetterOrDigit_Throw()
    {
        Assert.Throws<DomainValidationException>(() => new Username("!"));
    }

    [Fact]
    public void EmtpyString_Throw()
    {
        Assert.Throws<DomainValidationException>(() => new Username(""));
        Assert.Throws<DomainValidationException>(() => new Username(" "));
    }
}