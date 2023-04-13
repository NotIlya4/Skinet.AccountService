using System.Text;
using Api.Properties;

namespace Api.Extensions;

public static class ConfigurationConnectionExtensions
{
    public static string GetSqlServerConnectionString(this IConfiguration config, string? key = null)
    {
        if (key is not null)
        {
            config = config.GetSection(key);
        }

        string server = config.GetRequiredValue("Server");
        string database = config.GetRequiredValue("Database");
        string userId = config.GetRequiredValue("User Id");
        string password = config.GetRequiredValue("Password");
        string? multipleActiveResultSets = config["MultipleActiveResultSets"];
        string? trustServerCertificate = config["TrustServerCertificate"];

        StringBuilder connectionString =
            new StringBuilder($"Server={server};Database={database};User Id={userId};Password={password}");

        if (multipleActiveResultSets is not null)
        {
            connectionString.Append($";MultipleActiveResultSets={multipleActiveResultSets}");
        }

        if (trustServerCertificate is not null)
        {
            connectionString.Append($";TrustServerCertificate={trustServerCertificate}");
        }

        return connectionString.ToString();
    }

    public static string GetRedisConnectionString(this IConfiguration config, string? key = null)
    {
        if (key is not null)
        {
            config = config.GetSection(key);
        }
        
        string server = config.GetRequiredValue("Server");
        string? defaultDatabase = config["DefaultDatabase"];

        StringBuilder connectionString = new($"{server}");

        if (defaultDatabase is not null)
        {
            connectionString.Append($",defaultDatabase={defaultDatabase}");
        }

        return connectionString.ToString();
    }

    public static string GetRequiredValue(this IConfiguration config, string key)
    {
        return config.GetRequiredValue<string>(key);
    }
    
    public static T GetRequiredValue<T>(this IConfiguration config, string key)
    {
        T? value = config.GetValue<T>(key);
        if (value is null)
        {
            throw new InvalidOperationException(key);
        }
        return value;
    }
}