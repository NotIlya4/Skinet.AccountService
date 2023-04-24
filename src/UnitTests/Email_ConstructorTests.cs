using Domain.Exceptions;
using Domain.Primitives;

namespace UnitTests;

public class Email_ConstructorTests
{
    [Fact]
    public void OneCharacterBeforeAtSign_ValidEmail()
    {
        string email = "a@email.com";
        new Email(email);
    }

    [Fact]
    public void OnlyNumberBeforeAtSign_ValidEmail()
    {
        string email = "1@email.com";
        new Email(email);
    }

    [Fact]
    public void NotLetterOrDigitBeforeAtSign_ThrowDomainValidationException()
    {
        string email = "!@email.com";
        
        Assert.Throws<DomainValidationException>(() => new Email(email));
    }
}