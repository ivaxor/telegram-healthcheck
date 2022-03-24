using IVAXOR.TelegramHealthCheck.Models;
using IVAXOR.TelegramHealthCheck.Models.Configurations;
using IVAXOR.TelegramHealthCheck.Services.Interfaces;
using Microsoft.Extensions.Configuration;

namespace IVAXOR.TelegramHealthCheck.Services.Implementations;

public class HealthCheckService : IHealthCheckService
{
    private readonly IConfiguration _configuration;
    private readonly IEndpointStatusService _endpointStatusService;
    private readonly IHealthCheckResponseRepository _healthCheckResponseRepository;
    private readonly ITelegramService _telegramService;

    public HealthCheckService(
        IConfiguration configuration,
        IEndpointStatusService endpointStatusService,
        IHealthCheckResponseRepository healthCheckResponseRepository,
        ITelegramService telegramService)
    {
        _configuration = configuration;
        _endpointStatusService = endpointStatusService;
        _healthCheckResponseRepository = healthCheckResponseRepository;
        _telegramService = telegramService;
    }

    public string[] GetIds()
    {
        return GetHealthCheckConfigurations()
            .Select(_ => _.Id)
            .ToArray();
    }

    public async Task<HealthCheckRecord[]> UpdateAsync(CancellationToken cancellationToken = default)
    {
        return await GetHealthCheckConfigurations()
            .ToAsyncEnumerable()
            .SelectAwait(async _ => await UpdateAsync(_, cancellationToken))
            .ToArrayAsync(cancellationToken);
    }

    public async Task<HealthCheckRecord> UpdateAsync(string id, CancellationToken cancellationToken = default)
    {
        var configuration = GetHealthCheckConfigurationById(id);
        return await UpdateAsync(configuration, cancellationToken);
    }

    private async Task<HealthCheckRecord> UpdateAsync(
        HealthCheckConfiguration configuration,
        CancellationToken cancellationToken = default)
    {
        var response = await _endpointStatusService.CheckAsync(configuration.Url, cancellationToken);

        var record = new HealthCheckRecord(configuration.Id, response);
        var previousRecord = await _healthCheckResponseRepository.GetAsync(configuration.Id, cancellationToken);

        var outdatedAfter = GetOutdatedAfter();

        var isStatusChanged = previousRecord == null ||
                              previousRecord.StatusCode != record.StatusCode ||
                              record.CheckedAt - previousRecord.CheckedAt > outdatedAfter;
        if (isStatusChanged)
        {
            var message = response.StatusCode is >= 200 and < 300
                ? configuration.MessageWhenSucceeded
                : configuration.MessageWhenFailed
                    .Replace("{StatusCode}", response.StatusCode.ToString())
                    .Replace("{StatusText}", response.StatusText);

            await _telegramService.SendMessageAsync(
                configuration.TelegramChatId,
                configuration.TelegramBotApiKey,
                message,
                cancellationToken);
        }


        await _healthCheckResponseRepository.SaveAsync(record, cancellationToken);

        return record;
    }

    private HealthCheckConfiguration GetHealthCheckConfigurationById(string id)
    {
        return GetHealthCheckConfigurations()
            .Single(_ => string.Equals(_.Id, id, StringComparison.OrdinalIgnoreCase));
    }

    private HealthCheckConfiguration[] GetHealthCheckConfigurations()
    {
        return _configuration
            .GetSection(nameof(HealthCheckConfiguration))
            .Get<HealthCheckConfiguration[]>();
    }

    private TimeSpan GetOutdatedAfter()
    {
        return _configuration
            .GetSection(nameof(CosmosDbConfiguration))
            .Get<CosmosDbConfiguration>()
            .OutdatedAfter;
    }
}
