using Domain.Exceptions;

namespace Domain.Primitives;

public record Username
{
    public string Value { get; }

    public Username(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
        {
            throw DomainValidationException.CannotBeEmpty(nameof(Username));
        }
        
        if (ContainsOnlyLettersAndDigits(value))
        {
            throw new DomainValidationException("Username can consist only of letters and digits");
        }

        Value = value;
    }

    public override string ToString()
    {
        return Value;
    }

    private bool ContainsOnlyLettersAndDigits(string value)
    {
        return value.Any(s => !char.IsLetterOrDigit(s));
    }
}