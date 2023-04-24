namespace Domain.Exceptions;

public class DomainValidationException : Exception
{
    public DomainValidationException(string msg) : base(msg)
    {
        
    }

    public static DomainValidationException CannotBeEmpty(string entityName)
    {
        return new DomainValidationException($"{entityName} can not be empty");
    }
}