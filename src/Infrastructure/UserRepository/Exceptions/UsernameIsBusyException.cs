namespace Infrastructure.UserRepository.Exceptions;

public class UsernameIsBusyException : Exception
{
    public UsernameIsBusyException() : base("This username has been already taken")
    {
        
    }
}