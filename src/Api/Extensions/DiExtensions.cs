using System.ComponentModel.DataAnnotations;
using Api.ExeptionCatching;
using Api.Properties;
using ExceptionCatcherMiddleware.Extensions;
using Infrastructure.JwtTokenManager;
using Infrastructure.JwtTokenService;
using Infrastructure.RefreshTokenSystem;
using Infrastructure.RefreshTokenSystem.Repository;
using Infrastructure.UserService;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using StackExchange.Redis;

namespace Api.Extensions;

public static class DiExtensions
{
    public static void AddConfiguredExceptionCatcherMiddleware(this IServiceCollection services)
    {
        services.AddExceptionCatcherMiddlewareServices(builder =>
        {
            builder.RegisterExceptionMapper<ValidationException, ValidationExceptionMapper>();
        });
    }

    public static void AddUserService(this IServiceCollection services)
    {
        services.AddScoped<IUserService, UserService>();
    }

    public static void AddJwtTokenServices(this IServiceCollection services, JwtTokenManagerOptions jwtTokenManagerOptions)
    {
        services.AddScoped<IJwtTokenService, JwtTokenService>();
        services.AddScoped<IJwtTokenManager, JwtTokenManager>();
        services.AddSingleton(jwtTokenManagerOptions);
    }

    public static void AddRefreshTokenRepository(this IServiceCollection services, RefreshTokenRepositoryOptions options)
    {
        services.AddScoped<IRefreshTokenRepository, RefreshTokenRepository>();
        services.AddSingleton(options);
        services.AddScoped<RefreshTokenSerializer>();
    }

    public static void AddDbContextForIdentity(this IServiceCollection services, string connectionString)
    {
        services.AddDbContext<IdentityDbContext>(builder =>
        {
            builder.UseSqlServer(connectionString, b => b.MigrationsAssembly("Infrastructure"));
        });
    }
    
    public static void AddConfiguredIdentity(this IServiceCollection services)
    {
        services.AddIdentityCore<IdentityUser>().AddEntityFrameworkStores<IdentityDbContext>();
    }
}