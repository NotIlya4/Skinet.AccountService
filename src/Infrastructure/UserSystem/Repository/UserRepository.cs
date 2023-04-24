using Domain.Entities;
using Infrastructure.EntityFramework;
using Infrastructure.EntityFramework.Helpers;
using Infrastructure.EntityFramework.Models;
using Infrastructure.UserSystem.Repository.Extensions;

namespace Infrastructure.UserSystem.Repository;

public class UserRepository : IUserRepository
{
    private readonly AppDbContext _dbContext;
    private readonly DataMapper _dataMapper;

    public UserRepository(AppDbContext dbContext, DataMapper dataMapper)
    {
        _dbContext = dbContext;
        _dataMapper = dataMapper;
    }
    
    public async Task Insert(User user, string passwordHash)
    {
        UserData userData = _dataMapper.MapUser(user, passwordHash);
        await _dbContext.Users.AddAsync(userData);
        await _dbContext.SaveChangesAsync();
    }

    public async Task<User> Get(UserRepositoryStrictFilter strictFilter, string value)
    {
        UserData userData = await _dbContext.GetUser(strictFilter, value);
        User user = _dataMapper.MapUser(userData);
        return user;
    }

    public async Task<string> GetPasswordHash(Guid userId)
    {
        UserData userData = await _dbContext.GetUser(UserRepositoryStrictFilter.Id, userId.ToString());
        string passwordHash = userData.PasswordHash;
        return passwordHash;
    }
}