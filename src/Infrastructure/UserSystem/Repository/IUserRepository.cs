using Domain.Entities;

namespace Infrastructure.UserSystem.Repository;

public interface IUserRepository
{
    public Task Insert(User user, string passwordHash);
    public Task<User> Get(UserRepositoryStrictFilter strictFilter, string value);
    public Task<string> GetPasswordHash(Guid userId);
}