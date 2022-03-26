using IVAXOR.TelegramHealthCheck.Models;
using Microsoft.Azure.Cosmos;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace IVAXOR.TelegramHealthCheck.Infrastructure.Startup;

public static class CosmosDbStartup
{
    public static async Task AddAsync(IConfiguration configuration, IServiceCollection services)
    {
        var cosmosDbConfiguration = configuration
            .GetSection(nameof(CosmosDbConfiguration))
            .Get<CosmosDbConfiguration>();

        var cosmosClient = new CosmosClient(cosmosDbConfiguration.ConnectionString);
        await cosmosClient.CreateDatabaseIfNotExistsAsync(cosmosDbConfiguration.Database);
        services.AddSingleton(cosmosClient);

        var cosmosDatabase = cosmosClient.GetDatabase(cosmosDbConfiguration.Database);
        services.AddSingleton(cosmosDatabase);
    }
}
