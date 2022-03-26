using System.Net;
using IVAXOR.TelegramHealthCheck.Models;
using IVAXOR.TelegramHealthCheck.Models.Constants;
using IVAXOR.TelegramHealthCheck.Services.Interfaces;
using Microsoft.Azure.Cosmos;
using Microsoft.Extensions.Logging;

namespace IVAXOR.TelegramHealthCheck.Services.Implementations;

public class HealthCheckRecordRepository : IHealthCheckRecordRepository
{
    private readonly ILogger _logger;
    private readonly Container _cosmosDbContainer;

    public HealthCheckRecordRepository(
        ILogger<HealthCheckRecordRepository> logger,
        Database cosmosDbDatabase)
    {
        _logger = logger;

        _cosmosDbContainer = cosmosDbDatabase
            .CreateContainerIfNotExistsAsync(CosmosDbConstants.Records.ContainerName, CosmosDbConstants.Records.PartitionKeyPath)
            .GetAwaiter()
            .GetResult();
    }

    public async Task<HealthCheckRecord[]> GetAsync(CancellationToken cancellationToken = default)
    {
        _logger.LogInformation("Getting health check records");

        var response = await _cosmosDbContainer
            .GetItemQueryIterator<HealthCheckRecord>()
            .ReadNextAsync(cancellationToken);

        _logger.LogInformation("Health check records obtained");

        return response.ToArray();
    }

    public async Task<HealthCheckRecord?> GetAsync(string id, CancellationToken cancellationToken = default)
    {
        _logger.LogInformation("Getting specific health check record");

        try
        {
            var response = await _cosmosDbContainer.ReadItemAsync<HealthCheckRecord>(
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

    public async Task SaveAsync(HealthCheckRecord record, CancellationToken cancellationToken = default)
    {
        _logger.LogInformation("Saving health check record");

        try
        {
            await _cosmosDbContainer.ReadItemAsync<HealthCheckRecord>(
                record.Id,
                new PartitionKey(record.Id),
                cancellationToken: cancellationToken);

            await _cosmosDbContainer.ReplaceItemAsync(
                record,
                record.Id,
                new PartitionKey(record.Id),
                cancellationToken: cancellationToken);
        }
        catch (CosmosException exception) when (exception.StatusCode == HttpStatusCode.NotFound)
        {
            await _cosmosDbContainer.CreateItemAsync(
                record,
                new PartitionKey(record.Id),
                cancellationToken: cancellationToken);
        }
        finally
        {
            _logger.LogInformation("Health check record saved");
        }
    }
}
