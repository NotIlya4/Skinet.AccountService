using Domain.Primitives;

namespace Infrastructure.UserService.Helpers;

public interface IPasswordHasher
{
    public string Hash(Password password);
    public string Hash(string password);
    public bool Verify(Password password, string hash);
    public bool Verify(string password, string hash);
}