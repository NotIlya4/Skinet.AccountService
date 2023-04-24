using Domain.Exceptions;

namespace Domain.Primitives.Address;

public record Street
{
    public string Value { get; }

    public Street(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
        {
            throw DomainValidationException.CannotBeEmpty(nameof(Street));
        }

        Value = value;
    }

    public override string ToString()
    {
        return Value;
    }
};