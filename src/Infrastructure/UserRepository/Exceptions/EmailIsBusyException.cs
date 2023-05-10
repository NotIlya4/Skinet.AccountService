namespace Infrastructure.UserRepository.Exceptions;

public class EmailIsBusyException : Exception
{
    public EmailIsBusyException() : base("This email has been already taken")
    {
        
    }
}