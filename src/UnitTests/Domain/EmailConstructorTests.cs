namespace UnitTests.Domain;

public class EmailConstructorTests
{
    [Fact]
    public void OneCharacterBeforeAtSign_CreateInstance()
    {
        new Email("a@email.com");
    }

    [Fact]
    public void OnlyNumberBeforeAtSign_CreateInstance()
    {
        new Email("1@email.com");
    }

    [Fact]
    public void SpaceInEmail_Throw()
    {
        Assert.Throws<DomainValidationException>(() => new Email("a @email.com"));
    }

    [Fact]
    public void EmailWithoutAtSign_Throw()
    {
        Assert.Throws<DomainValidationException>(() => new Email("aemail.com"));
    }

    [Fact]
    public void NotLetterOrDigitBeforeAtSign_Throw()
    {
        Assert.Throws<DomainValidationException>(() => new Email("!.&^@email.com"));
    }
}