using Infrastructure.EntityFramework;
using Infrastructure.EntityFramework.Helpers;
using Infrastructure.EntityFramework.Models;
using Infrastructure.UserRepository.Exceptions;
using Infrastructure.UserRepository.Extensions;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.UserRepository;

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
        
        try
        {
            await _dbContext.GetUser(UserRepositoryStrictFilter.Id, user.Id.ToString());
            _dbContext.Users.Update(userData);

        }
        catch (UserNotFoundException)
        {
            await _dbContext.Users.AddAsync(userData);
        }

        try
        {
            await _dbContext.SaveChangesAsync();
        }
        catch (DbUpdateException e)
        {
            if (e.InnerException is not null && e.InnerException.Message.Contains("IX_Users_Email"))
            {
                throw new EmailIsBusyException();
            }
            
            if (e.InnerException is not null && e.InnerException.Message.Contains("IX_Users_Username"))
            {
                throw new UsernameIsBusyException();
            }

            throw;
        }
    }

    public async Task<User> GetById(UserId id)
    {
        UserData userData = await _dbContext.GetUser(UserRepositoryStrictFilter.Id, id.Value.ToString());
        User user = _dataMapper.MapUser(userData);
        return user;
    }

    public async Task<User> GetByUsername(Username username)
    {
        UserData userData = await _dbContext.GetUser(UserRepositoryStrictFilter.Username, username.Value);
        User user = _dataMapper.MapUser(userData);
        return user;
    }

    public async Task<User> GetByEmail(Email email)
    {
        UserData userData = await _dbContext.GetUser(UserRepositoryStrictFilter.Email, email.Value);
        User user = _dataMapper.MapUser(userData);
        return user;
    }

    public async Task<string> GetPasswordHash(UserId userId)
    {
        UserData userData = await _dbContext.GetUser(UserRepositoryStrictFilter.Id, userId.Value.ToString());
        string passwordHash = userData.PasswordHash;
        return passwordHash;
    }
}