# AccountService
This is a REST API Service for YetAnotherMarketplace that provides user managing endpoints. Its responsible for registering new users, login them and handle their sessions using jwt and refresh tokens.

## Environment Variables
Service contains several environment variables that you can change to control its behavior:
- `AutoMigrate` When set to `true`, the service will automatically apply any necessary database migrations on startup. When set to `false`, you must apply the migrations manually.
- `AutoSeed` When set to `true`, the service will automatically seed the database with sample data on startup.
- `JwtTokenHelperOptions__Secret` Secret for jwt token signs.
- `JwtTokenHelperOptions__ExpireMinutes` Controls timespan, in minutes, after which jwt token expires.
- `RefreshTokenRepositoryOptions__ExpireHours` Controls timespan, in hours, after which refresh token expires.
- `SeqUrl` Seq url for logging.
- `Serilog` Override serilog configuration.
- `SqlServer__Server` Sql server url.
- `SqlServer__Database` Sql server database.
- `SqlServer__User` Id Sql server user.
- `SqlServer__Password` Sql server user's password.

## Database Migrations
You can apply migrations manually. Container contains entity framework migrations bundle at `/app/efbundle`. You can either run script `ApplyMigrations.py` or command yourself:
```
docker exec -it product-service /app/efbundle
```
