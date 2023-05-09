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
        
        if (value.Any(s => !char.IsLetterOrDigit(s)))
        {
            throw new DomainValidationException("Username can consist only of letters and digits");
        }

        Value = value;
    }

    public override string ToString()
    {
        return Value;
    }
}