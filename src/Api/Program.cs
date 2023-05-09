using Api.Extensions;
using Api.Properties;
using ExceptionCatcherMiddleware.Extensions;
using Serilog;

var builder = WebApplication.CreateBuilder(args);
var services = builder.Services;
ParametersProvider parameters = new(builder.Configuration);

services.AddRepositories(parameters.RefreshTokenRepositoryOptions);
services.AddRedis(parameters.Redis);
services.AddConfiguredExceptionCatcherMiddleware();
services.AddConfiguredDbContext(parameters.SqlServer);
services.AddMappers();
services.AddServices(parameters.JwtTokenHelperOptions);
services.AddConfiguredSwaggerGen();
builder.AddConfiguredSerilog(parameters.Seq);

services.AddControllers();
services.AddEndpointsApiExplorer();

WebApplication app = builder.Build();

app.ConfigureDb(parameters);

app.UseSerilogRequestLogging();
app.UseExceptionCatcherMiddleware();
app.UseSwagger();
app.UseSwaggerUI();
app.MapControllers();

app.Run();
