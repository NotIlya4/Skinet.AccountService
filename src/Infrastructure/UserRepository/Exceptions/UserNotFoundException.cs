namespace Infrastructure.UserRepository.Exceptions;

public class UserNotFoundException : Exception
{
    public UserNotFoundException() : base("Specified user not found")
    {
        
    }
}