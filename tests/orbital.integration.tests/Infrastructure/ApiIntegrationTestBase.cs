using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using orbital.core.Data;

namespace orbital.integration.tests.Infrastructure;

/// <summary>
/// Base class for API integration tests.
/// Provides a configured WebApplicationFactory with in-memory dependencies.
/// </summary>
public class ApiIntegrationTestBase : IClassFixture<WebApplicationFactory<Program>>
{
    protected readonly WebApplicationFactory<Program> Factory;
    protected readonly HttpClient Client;

    public ApiIntegrationTestBase(WebApplicationFactory<Program> factory)
    {
        // Set environment variable to provide Cosmos DB connection for test environment
        Environment.SetEnvironmentVariable("ConnectionStrings__orbital", 
            "AccountEndpoint=https://localhost:8081/;AccountKey=C2y6yDjf5/R+ob0N8A7Cgv30VRDJIWEHLM+4QDU5DE2nQ9nDuVTqobD4b8mGGyPMbIZnqyMsEcaGQy67XIw/Jw==");
        
        Factory = factory.WithWebHostBuilder(builder =>
        {
            builder.UseEnvironment("Testing");
            
            builder.ConfigureServices(services =>
            {
                // Remove all Cosmos DB related services to prevent actual database connections
                services.RemoveAll(typeof(Microsoft.Azure.Cosmos.Database));
                services.RemoveAll(typeof(Microsoft.Azure.Cosmos.Container));
                services.RemoveAll(typeof(Microsoft.Azure.Cosmos.CosmosClient));
                
                // In Testing environment, repositories are not registered in Program.cs
                // So we just add our in-memory implementations
                services.AddScoped<IMeetingRepository, InMemoryMeetingRepository>();
                services.AddScoped<IMetadataRepository, InMemoryMetadataRepository>();
            });
        });

        Client = Factory.CreateClient();
    }
}
