using Infrastructure.EntityFramework;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;

namespace IntegrationTests.Setup;

[CollectionDefinition(nameof(AppFixture))]
public class AppFixture : IAsyncLifetime, ICollectionFixture<AppFixture>
{
    internal WebApplicationFactory<Program> Fixture { get; private set; } = null!;
    private IServiceScope _scope = null!;
    public SqlDbHelper Helper { get; private set; } = null!;
    
    public async Task InitializeAsync()
    {
        Fixture = new WebApplicationFactory<Program>().WithWebHostBuilder(builder =>
        {
            builder.UseEnvironment("IntegrationTests");
        });
        _scope = Fixture.Services.CreateScope();
        Helper = new SqlDbHelper(_scope.ServiceProvider.GetRequiredService<AppDbContext>());

        await Helper.Reload();
    }

    public IServiceScope CreateScope()
    {
        return Fixture.Services.CreateScope();
    }

    public async Task DisposeAsync()
    {
        await Helper.Drop();
        _scope.Dispose();
    }
}