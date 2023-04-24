using Domain.Exceptions;

namespace Domain.Primitives.Address;

public record Country
{
    public string Value { get; }

    public Country(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
        {
            throw DomainValidationException.CannotBeEmpty(nameof(Country));
        }

        Value = value;
    }

    public override string ToString()
    {
        return Value;
    }
}