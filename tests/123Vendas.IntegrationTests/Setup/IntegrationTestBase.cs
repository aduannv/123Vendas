using _123Vendas.Infrastructure.Data;
using _123Vendas.IntegrationTests.Extensions;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Testcontainers.PostgreSql;

namespace _123Vendas.IntegrationTests.Setup;

public class IntegrationTestBase : IClassFixture<WebApplicationFactory<Program>>, IAsyncLifetime
{
    private readonly WebApplicationFactory<Program> _factory;
    private readonly PostgreSqlContainer _postgreSqlContainer = new PostgreSqlBuilder()
                        .WithName(Guid.NewGuid().ToString())
                        .WithImage("postgres:latest")
                        .WithDatabase("123vendas.database")
                        .WithUsername("postgres")
                        .WithPassword("postgres")
                        .Build();

    public HttpClient _client;

    public IntegrationTestBase(WebApplicationFactory<Program> factory)
    {
        _factory = factory;
    }

    public async Task InitializeAsync()
    {
        await _postgreSqlContainer.StartAsync();

        _client = _factory.WithWebHostBuilder(builder =>
        {
            builder.ConfigureTestServices(services =>
            {
                services.Remove<DbContextOptions<SalesDbContext>>();

                services.AddDbContext<SalesDbContext>(options =>
                {
                    options.UseNpgsql(_postgreSqlContainer.GetConnectionString());
                });
            });
        }).CreateClient();
    }

    public async new Task DisposeAsync()
    {
        await _postgreSqlContainer.StopAsync();
    }
}
