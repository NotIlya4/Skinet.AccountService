using Api.Extensions;
using Infrastructure.JwtTokenHelper;
using Infrastructure.RefreshTokenService.Models;

namespace Api.Properties;

public class ParametersProvider
{
    private readonly IConfiguration _config;

    public ParametersProvider(IConfiguration config)
    {
        _config = config;
    }

    public RefreshTokenRepositoryOptions RefreshTokenRepositoryOptions => _config.GetRefreshTokenRepositoryOptions("RefreshTokenRepositoryOptions");

    public JwtTokenHelperOptions JwtTokenHelperOptions => _config.GetJwtTokenManagerOptions("JwtTokenHelperOptions");

    public string SqlServer => _config.GetSqlServerConnectionString("SqlServer");
    
    public string Redis => _config.GetRedisConnectionString("Redis");

    public string Seq => _config.GetRequiredValue<string>("SeqUrl");

    public bool AutoMigrate => _config.GetRequiredValue<bool>("AutoMigrate");

    public bool AutoSeed => _config.GetRequiredValue<bool>("AutoSeed");
}