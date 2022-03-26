using System.Net;
using IVAXOR.TelegramHealthCheck.Models;
using IVAXOR.TelegramHealthCheck.Models.Constants;
using IVAXOR.TelegramHealthCheck.Services.Interfaces;
using Microsoft.Azure.Cosmos;
using Microsoft.Azure.Cosmos.Linq;
using Microsoft.Extensions.Logging;

namespace IVAXOR.TelegramHealthCheck.Services.Implementations;

public class HealthCheckConfigurationRepository : IHealthCheckConfigurationRepository
{
    private readonly ILogger _logger;
    private readonly Container _cosmosDbContainer;

    public HealthCheckConfigurationRepository(
        ILogger<HealthCheckConfigurationRepository> logger,
        Database cosmosDbDatabase)
    {
        _logger = logger;

        _cosmosDbContainer = cosmosDbDatabase
            .CreateContainerIfNotExistsAsync(CosmosDbConstants.Configurations.ContainerName, CosmosDbConstants.Configurations.PartitionKeyPath)
            .GetAwaiter()
            .GetResult();

        SeedContainerIfEmptyAsync()
            .GetAwaiter()
            .GetResult();
    }

    public async Task<HealthCheckConfiguration[]> GetAsync(CancellationToken cancellationToken = default)
    {
        _logger.LogInformation("Getting health check configurations");

        var response = await _cosmosDbContainer
            .GetItemQueryIterator<HealthCheckConfiguration>()
            .ReadNextAsync(cancellationToken);

        _logger.LogInformation("Health check configurations obtained");

        return response.ToArray();
    }

    public async Task<HealthCheckConfiguration?> GetAsync(string id, CancellationToken cancellationToken = default)
    {
        _logger.LogInformation("Getting specific health check record");

        try
        {
            var response = await _cosmosDbContainer.ReadItemAsync<HealthCheckConfiguration>(
                id,
                new PartitionKey(id),
                cancellationToken: cancellationToken);

            return response.Resource;
        }
        catch (CosmosException exception) when (exception.StatusCode == HttpStatusCode.NotFound)
        {
            return null;
        }
        finally
        {
            _logger.LogInformation("Specific health check record obtained");
        }
    }

    private async Task SeedContainerIfEmptyAsync(CancellationToken cancellationToken = default)
    {
        var configurationCount = await _cosmosDbContainer
            .GetItemLinqQueryable<HealthCheckConfiguration>()
            .CountAsync(cancellationToken);
        if (configurationCount != 0) return;

        var configuration = new HealthCheckConfiguration()
        {
            Id = "Google",
            Url = "https://google.com",
            SendMessageAfterDelayMoreThen = TimeSpan.FromDays(1),
            TelegramChatId = "-1",
            TelegramBotApiKey = "bot:key",
            MessageWhenSucceeded = "I feel lucky",
            MessageWhenFailed = "Ops, internet are domed"
        };

        await _cosmosDbContainer.CreateItemAsync(
            configuration,
            new PartitionKey(configuration.Id),
            cancellationToken: cancellationToken);
    }
}
