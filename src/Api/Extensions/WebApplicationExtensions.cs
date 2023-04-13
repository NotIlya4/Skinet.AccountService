using Api.Properties;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Api.Extensions;

public static class WebApplicationExtensions
{
    public static void ConfigureDb(this WebApplication app, ParametersProvider parametersProvider)
    {
        if (parametersProvider.AutoMigrate())
        {
            var scope = app.Services.CreateScope();
            IdentityDbContext dbContext = scope.ServiceProvider.GetRequiredService<IdentityDbContext>();
            dbContext.Database.Migrate();
            scope.Dispose();
        }
    }
}