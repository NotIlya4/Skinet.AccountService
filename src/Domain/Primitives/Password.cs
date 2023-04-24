using Domain.Exceptions;

namespace Domain.Primitives;

public record Password
{
    public string Value { get; }
    private const int PasswordLength = 8;

    public Password(string value)
    {
        if (value.Length < PasswordLength)
        {
            throw new DomainValidationException($"Password must be {PasswordLength} symbols or longer");
        }

        if (!value.Any(char.IsLetter))
        {
            throw new DomainValidationException("Password must contain at least one letter");
        }

        if (!value.Any(char.IsDigit))
        {
            throw new DomainValidationException("Password must contain at least one digit");
        }

        if (!value.Any(char.IsUpper))
        {
            throw new DomainValidationException("Password must contain at least one capital letter");
        }

        if (!value.Any(char.IsLower))
        {
            throw new DomainValidationException("Password must contain at least one lower case letter");
        }

        Value = value;
    }

    public override string ToString()
    {
        return Value;
    }
};