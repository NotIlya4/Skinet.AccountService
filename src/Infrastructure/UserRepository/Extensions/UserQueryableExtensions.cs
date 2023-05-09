using System.Linq.Dynamic.Core;
using Infrastructure.EntityFramework;
using Infrastructure.EntityFramework.Extensions;
using Infrastructure.EntityFramework.Models;

namespace Infrastructure.UserRepository.Extensions;

public static class UserQueryableExtensions
{
    public static async Task<UserData> GetUser(this AppDbContext dbContext, UserRepositoryStrictFilter strictFilter, string value)
    {
        return await dbContext.Users.GetUser(strictFilter, value);
    }
    
    public static async Task<UserData> GetUser(this IQueryable<UserData> query, UserRepositoryStrictFilter strictFilter, string value)
    {
        string property = strictFilter.ToString();
        return await query
            .Where($"u => u.{property} == \"{value}\"")
            .FirstAsyncOrThrow();
    }
}