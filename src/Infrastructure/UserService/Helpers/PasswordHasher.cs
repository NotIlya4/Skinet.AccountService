using BC = BCrypt.Net.BCrypt;

namespace Infrastructure.UserService.Helpers;

public class PasswordHasher : IPasswordHasher
{
    public string Hash(Password password)
    {
        return Hash(password.Value);
    }

    public string Hash(string password)
    {
        return BC.HashPassword(password);
    }

    public bool Verify(Password password, string hash)
    {
        return Verify(password.Value, hash);
    }

    public bool Verify(string password, string hash)
    {
        return BC.Verify(password, hash);
    }
}