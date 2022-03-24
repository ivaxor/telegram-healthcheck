using IVAXOR.TelegramHealthCheck.Models.Configurations;
using IVAXOR.TelegramHealthCheck.Models.Constants;
using Microsoft.Azure.Cosmos;

namespace IVAXOR.TelegramHealthCheck.Web.Infrastructure;

internal static class CosmosDbStartup
{
    public static async Task AddCosmosDbAsync(this WebApplicationBuilder builder)
    {
        var cosmosDbConfiguration = builder.Configuration
            .GetSection(nameof(CosmosDbConfiguration))
            .Get<CosmosDbConfiguration>();

        var cosmosClient = new CosmosClient(cosmosDbConfiguration.ConnectionString);
        await cosmosClient.CreateDatabaseIfNotExistsAsync(cosmosDbConfiguration.Database);
        builder.Services.AddSingleton(cosmosClient);

        var cosmosDatabase = cosmosClient.GetDatabase(cosmosDbConfiguration.Database);
        builder.Services.AddSingleton(cosmosDatabase);

        var cosmosContainer = await cosmosDatabase.CreateContainerIfNotExistsAsync(
            CosmosDbConstants.ContainerName,
            CosmosDbConstants.PartitionKeyPath);
        builder.Services.AddSingleton(cosmosContainer.Container);
    }
}
