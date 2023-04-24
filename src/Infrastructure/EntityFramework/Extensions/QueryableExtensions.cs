using System.Linq.Expressions;
using Infrastructure.UserSystem.Repository.Exceptions;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.EntityFramework.Extensions;

public static class QueryableExtensions
{
    public static async Task<T> FirstAsyncOrThrow<T>(this IQueryable<T> query, Expression<Func<T, bool>> predicate)
    {
        return await query.FirstOrDefaultAsync(predicate) ?? throw new UserNotFoundException();
    }
    
    public static async Task<T> FirstAsyncOrThrow<T>(this IQueryable<T> query)
    {
        return await query.FirstOrDefaultAsync() ?? throw new UserNotFoundException();
    }
}