using System.ComponentModel.DataAnnotations;
using Api.ExceptionMappers;
using Api.UserController.Helpers;
using ExceptionCatcherMiddleware.Extensions;
using Infrastructure.EntityFramework;
using Infrastructure.EntityFramework.Models;
using Infrastructure.JwtTokenManager;
using Infrastructure.JwtTokenService;
using Infrastructure.RefreshTokenSystem;
using Infrastructure.RefreshTokenSystem.Repository;
using Infrastructure.UserService;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using StackExchange.Redis;

namespace Api.Extensions;

public static class DiExtensions
{
    public static void AddConfiguredExceptionCatcherMiddleware(this IServiceCollection services)
    {
        services.AddExceptionCatcherMiddlewareServices(builder =>
        {
            builder.RegisterExceptionMapper<SecurityTokenExpiredException, SecurityTokenExpiredExceptionMapper>();
            builder.RegisterExceptionMapper<ValidationException, ValidationExceptionMapper>();
        });
    }

    public static void AddUserService(this IServiceCollection services)
    {
        services.AddScoped<IUserService, UserService>();
    }

    public static void AddMappers(this IServiceCollection services)
    {
        services.AddScoped<ViewMapper>();
    }

    public static void AddJwtTokenServices(this IServiceCollection services, JwtTokenManagerOptions jwtTokenManagerOptions)
    {
        services.AddScoped<IJwtTokenService, JwtTokenService>();
        services.AddScoped<IJwtTokenManager, JwtTokenManager>();
        services.AddSingleton(jwtTokenManagerOptions);
    }

    public static void AddRedisForRefreshTokenRepository(this IServiceCollection services, string redisConnectionString)
    {
        services.AddSingleton(_ =>
        {
            ConnectionMultiplexer connectionMultiplexer = ConnectionMultiplexer.Connect(redisConnectionString);
            return connectionMultiplexer.GetDatabase();
        });
    }

    public static void AddRefreshTokenRepository(this IServiceCollection services, RefreshTokenRepositoryOptions options)
    {
        services.AddScoped<IRefreshTokenRepository, RefreshTokenRepository>();
        services.AddSingleton(options);
        services.AddScoped<RefreshTokenSerializer>();
    }

    public static void AddDbContextForIdentity(this IServiceCollection services, string connectionString)
    {
        services.AddDbContext<AppDbContext>(builder =>
        {
            builder.UseSqlServer(connectionString, b => b.MigrationsAssembly("Infrastructure"));
        });
    }
    
    public static void AddConfiguredIdentity(this IServiceCollection services)
    {
        services.AddIdentityCore<UserData>()
            .AddEntityFrameworkStores<AppDbContext>();
    }
}