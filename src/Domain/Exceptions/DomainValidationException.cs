namespace Domain.Exceptions;

public class DomainValidationException : Exception
{
    public DomainValidationException(string msg) : base(msg)
    {
        
    }
}