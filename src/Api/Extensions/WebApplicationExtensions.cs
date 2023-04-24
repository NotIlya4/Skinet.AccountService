using Api.Properties;
using Infrastructure.EntityFramework;
using Microsoft.EntityFrameworkCore;

namespace Api.Extensions;

public static class WebApplicationExtensions
{
    public static void ConfigureDb(this WebApplication app, ParametersProvider parametersProvider)
    {
        if (parametersProvider.AutoMigrate())
        {
            var scope = app.Services.CreateScope();
            var dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();
            dbContext.Database.Migrate();
            scope.Dispose();
        }
    }
}