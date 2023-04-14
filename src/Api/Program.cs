using Api.Extensions;
using Api.Properties;
using ExceptionCatcherMiddleware.Extensions;

var builder = WebApplication.CreateBuilder(args);
var services = builder.Services;
ParametersProvider parametersProvider = new(builder.Configuration);

services.AddConfiguredIdentity();
services.AddDbContextForIdentity(parametersProvider.GetSqlServer());

services.AddRefreshTokenRepository(parametersProvider.GetRefreshTokenRepositoryOptions());
services.AddJwtTokenServices(parametersProvider.GetJwtTokenManagerOptions());
services.AddRedisForRefreshTokenRepository(parametersProvider.GetRedis());

services.AddMappers();
services.AddUserService();

services.AddControllers();
services.AddEndpointsApiExplorer();
services.AddSwaggerGen(options => { options.DescribeAllParametersInCamelCase(); });
services.AddConfiguredExceptionCatcherMiddleware();

var app = builder.Build();

app.ConfigureDb(parametersProvider);

app.UseExceptionCatcherMiddleware();
app.UseSwagger();
app.UseSwaggerUI();
app.MapControllers();

app.Run();
