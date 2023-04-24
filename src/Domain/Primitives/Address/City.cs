using Domain.Exceptions;

namespace Domain.Primitives.Address;

public record City
{
    public string Value { get; }

    public City(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
        {
            throw DomainValidationException.CannotBeEmpty(nameof(City));
        }

        Value = value;
    }

    public override string ToString()
    {
        return Value;
    }
};