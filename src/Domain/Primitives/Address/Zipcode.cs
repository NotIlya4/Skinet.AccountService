using Domain.Exceptions;

namespace Domain.Primitives.Address;

public record Zipcode
{
    public string Value { get; }

    public Zipcode(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
        {
            throw DomainValidationException.CannotBeEmpty(nameof(Zipcode));
        }

        Value = value;
    }

    public override string ToString()
    {
        return Value;
    }
}