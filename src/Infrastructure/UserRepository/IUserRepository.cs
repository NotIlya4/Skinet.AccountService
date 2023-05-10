namespace Infrastructure.UserRepository;

public interface IUserRepository
{
    public Task Insert(User user, string passwordHash);
    public Task<User> GetById(UserId id);
    public Task<User> GetByUsername(Username username);
    public Task<User> GetByEmail(Email email);
    public Task<string> GetPasswordHash(UserId userId);
}