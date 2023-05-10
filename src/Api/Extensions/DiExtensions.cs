using System.ComponentModel.DataAnnotations;
using Api.ExceptionCatching;
using Api.UserController.Helpers;
using Domain.Exceptions;
using ExceptionCatcherMiddleware.Extensions;
using Infrastructure.EntityFramework;
using Infrastructure.EntityFramework.Helpers;
using Infrastructure.JwtTokenHelper;
using Infrastructure.JwtTokenPairService;
using Infrastructure.RefreshTokenService;
using Infrastructure.RefreshTokenService.Helpers;
using Infrastructure.RefreshTokenService.Models;
using Infrastructure.UserRepository;
using Infrastructure.UserService;
using Infrastructure.UserService.Helpers;
using Infrastructure.ValidJwtTokenSystem;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Serilog;
using Serilog.Events;
using StackExchange.Redis;
using SwaggerEnrichers.Extensions;

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

    public static void AddConfiguredSwaggerGen(this IServiceCollection services)
    {
        services.AddSwaggerGen(options =>
        {
            options.AddEnricherFilters();
            options.DescribeAllParametersInCamelCase();
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

    public static void AddConfiguredSerilog(this WebApplicationBuilder builder, string seqUrl)
    {
        builder.Services.AddHttpContextAccessor();
        Log.Logger = new LoggerConfiguration()
            .ReadFrom.Configuration(builder.Configuration)
            .MinimumLevel.Override("Microsoft.AspNetCore", LogEventLevel.Warning)
            .Enrich.FromLogContext()
            .Enrich.WithCorrelationIdHeader("x-request-id")
            .WriteTo.Console()
            .WriteTo.Seq(seqUrl)
            .Enrich.WithProperty("ServiceName", "AccountService")
            .CreateLogger();
        builder.Host.UseSerilog();
    }

    public static void AddServices(this IServiceCollection services, JwtTokenHelperOptions jwtTokenManagerOptions)
    {
        services.AddScoped<IUserService, UserService>();
        services.AddScoped<IPasswordHasher, PasswordHasher>();
        services.AddScoped<IUserRepository, UserRepository>();
        services.AddScoped<IJwtTokenPairService, JwtTokenPairService>();
        services.AddScoped<IJwtTokenHelper, JwtTokenHelper>();
        services.AddScoped<IValidJwtTokenFactory, ValidJwtTokenFactory>();
        services.AddScoped<IJwtTokenValidator, JwtTokenHelper>();
        services.AddSingleton(jwtTokenManagerOptions);
    }

    public static void AddMappers(this IServiceCollection services)
    {
        services.AddScoped<ViewMapper>();
        services.AddScoped<DataMapper>();
    }

    public static void AddRedis(this IServiceCollection services, string redisConnectionString)
    {
        services.AddSingleton(_ =>
        {
            ConnectionMultiplexer connectionMultiplexer = ConnectionMultiplexer.Connect(redisConnectionString);
            return connectionMultiplexer.GetDatabase();
        });
    }

    public static void AddRepositories(this IServiceCollection services, RefreshTokenRepositoryOptions options)
    {
        services.AddScoped<IRefreshTokenService, RefreshTokenService>();
        services.AddSingleton(options);
        services.AddScoped<RefreshTokenSerializer>();
    }
}