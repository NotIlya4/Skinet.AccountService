using Domain.Exceptions;

namespace Domain.Primitives;

public record Email
{
    public string Value { get; }

    public Email(string value)
    {
        if (value.Contains(" "))
        {
            throw new DomainValidationException("Email can't contain any spaces");
        }

        if (!value.Contains("@"))
        {
            throw new DomainValidationException("Email must contain @ symbol");
        }

        int atSymbolIndex = value.IndexOf("@", StringComparison.Ordinal) + 1;
        if (!value[0..atSymbolIndex].Any(char.IsLetterOrDigit))
        {
            throw new DomainValidationException("Email must contain at least 1 character or digit before @ symbol");
        }

        Value = value;
    }

    public override string ToString()
    {
        return Value;
    }
};