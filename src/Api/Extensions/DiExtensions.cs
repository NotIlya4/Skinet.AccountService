using System.ComponentModel.DataAnnotations;
using Api.ExceptionMappers;
using Api.UserController.Helpers;
using Domain.Exceptions;
using ExceptionCatcherMiddleware.Extensions;
using Infrastructure.EntityFramework;
using Infrastructure.EntityFramework.Helpers;
using Infrastructure.JwtTokenSystem.Manager;
using Infrastructure.JwtTokenSystem.Service;
using Infrastructure.RefreshTokenSystem;
using Infrastructure.RefreshTokenSystem.Repository;
using Infrastructure.UserSystem.Repository;
using Infrastructure.UserSystem.Service;
using Infrastructure.UserSystem.Service.Helpers;
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
            builder.RegisterExceptionMapper<DomainValidationException, DomainValidationExceptionMapper>();
        });
    }

    public static void AddConfiguredDbContext(this IServiceCollection services, string connectionString)
    {
        services.AddDbContext<AppDbContext>(builder =>
        {
            builder.UseSqlServer(connectionString, b => b.MigrationsAssembly("Infrastructure"));
            builder.UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking);
        });
    }

    public static void AddUserService(this IServiceCollection services)
    {
        services.AddScoped<IUserService, UserService>();
        services.AddScoped<IPasswordHasher, PasswordHasher>();
        services.AddScoped<IUserRepository, UserRepository>();
    }

    public static void AddMappers(this IServiceCollection services)
    {
        services.AddScoped<ViewMapper>();
        services.AddScoped<DataMapper>();
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
}