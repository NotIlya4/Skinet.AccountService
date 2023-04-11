using System.ComponentModel.DataAnnotations;
using Api.ExeptionCatching;
using ExceptionCatcherMiddleware.Extensions;

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
}