using Domain.Exceptions;
using Domain.Primitives;

namespace UnitTests.Domain;

public class PasswordConstructorTests
{
    [Fact]
    public void PasswordWithUpperAndLowerCase8Length_CreateInstance()
    {
        new Password("Passwor1");
    }

    [Fact]
    public void Password7Length_Throw()
    {
        Assert.Throws<DomainValidationException>(() => new Password("Passwo1"));
    }

    [Fact]
    public void PasswordWithoutNumber_Throw()
    {
        Assert.Throws<DomainValidationException>(() => new Password("Password"));
    }

    [Fact]
    public void PasswordWithoutUpperCaseLetters_Throw()
    {
        Assert.Throws<DomainValidationException>(() => new Password("passwor1"));
    }

    [Fact]
    public void PasswordWithoutLowerCaseLetters_Throw()
    {
        Assert.Throws<DomainValidationException>(() => new Password("PASSWOR1"));
    }

    [Fact]
    public void PasswordWithoutLetters_Throw()
    {
        Assert.Throws<DomainValidationException>(() => new Password("12345678"));
    }
}