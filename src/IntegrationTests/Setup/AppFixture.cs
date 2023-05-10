using Infrastructure.EntityFramework;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;
using StackExchange.Redis;

namespace IntegrationTests.Setup;

[CollectionDefinition(nameof(AppFixture))]
public class AppFixture : IAsyncLifetime, ICollectionFixture<AppFixture>
{
    internal WebApplicationFactory<Program> Fixture { get; private set; } = null!;
    private IServiceScope _scope = null!;
    public SqlDbHelper SqlDbHelper { get; private set; } = null!;
    public RedisHelper RedisHelper { get; private set; } = null!;
    public IDatabase Redis { get; private set; } = null!;
    
    public async Task InitializeAsync()
    {
        Fixture = new WebApplicationFactory<Program>().WithWebHostBuilder(builder =>
        {
            builder.UseEnvironment("IntegrationTests");
        });
        _scope = Fixture.Services.CreateScope();
        SqlDbHelper = new SqlDbHelper(_scope.ServiceProvider.GetRequiredService<AppDbContext>());
        Redis = _scope.ServiceProvider.GetRequiredService<IDatabase>();
        RedisHelper = new RedisHelper(Redis);

        await RedisHelper.Reload();
        await SqlDbHelper.Reload();
    }

    public IServiceScope CreateScope()
    {
        return Fixture.Services.CreateScope();
    }

    public async Task DisposeAsync()
    {
        await SqlDbHelper.Drop();
        _scope.Dispose();
    }
}