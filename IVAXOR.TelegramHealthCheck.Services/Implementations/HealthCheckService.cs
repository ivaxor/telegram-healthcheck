using IVAXOR.TelegramHealthCheck.Models;
using IVAXOR.TelegramHealthCheck.Models.Configurations;
using IVAXOR.TelegramHealthCheck.Models.Constants;
using IVAXOR.TelegramHealthCheck.Services.Interfaces;

namespace IVAXOR.TelegramHealthCheck.Services.Implementations;

public class HealthCheckService : IHealthCheckService
{
    private readonly IHealthCheckConfigurationProvider _healthCheckConfigurationProvider;
    private readonly IEndpointStatusService _endpointStatusService;
    private readonly IHealthCheckResponseRepository _healthCheckResponseRepository;
    private readonly ITelegramService _telegramService;

    public HealthCheckService(
        IHealthCheckConfigurationProvider healthCheckConfigurationProvider,
        IEndpointStatusService endpointStatusService,
        IHealthCheckResponseRepository healthCheckResponseRepository,
        ITelegramService telegramService)
    {
        _healthCheckConfigurationProvider = healthCheckConfigurationProvider;
        _endpointStatusService = endpointStatusService;
        _healthCheckResponseRepository = healthCheckResponseRepository;
        _telegramService = telegramService;
    }
    
    public async Task<HealthCheckRecord[]> UpdateAsync(CancellationToken cancellationToken = default)
    {
        return await _healthCheckConfigurationProvider.Get()
            .ToAsyncEnumerable()
            .SelectAwait(async _ => await UpdateAsync(_, cancellationToken))
            .ToArrayAsync(cancellationToken);
    }

    public async Task<HealthCheckRecord> UpdateAsync(string id, CancellationToken cancellationToken = default)
    {
        var configuration = _healthCheckConfigurationProvider.Get(id);
        return await UpdateAsync(configuration, cancellationToken);
    }

    public async Task<HealthCheckRecord> UpdateAsync(
        HealthCheckConfiguration configuration,
        CancellationToken cancellationToken = default)
    {
        var response = await _endpointStatusService.CheckAsync(configuration.Url, cancellationToken);

        var record = new HealthCheckRecord(configuration.Id, response);
        var previousRecord = await _healthCheckResponseRepository.GetAsync(configuration.Id, cancellationToken);

        var outdatedAfter = _healthCheckConfigurationProvider.GetOutdatedAfter();

        var isStatusChanged = previousRecord == null ||
                              previousRecord.StatusCode != record.StatusCode ||
                              record.CheckedAt - previousRecord.CheckedAt > outdatedAfter;
        if (isStatusChanged)
        {
            var message = response.StatusCode is >= 200 and < 300
                ? configuration.MessageWhenSucceeded
                : configuration.MessageWhenFailed
                    .Replace(TelegramMessageConstants.StatusCodeReplacePlaceholder, response.StatusCode.ToString())
                    .Replace(TelegramMessageConstants.StatusTextReplacePlaceholder, response.StatusText);

            await _telegramService.SendMessageAsync(
                configuration.TelegramChatId,
                configuration.TelegramBotApiKey,
                message,
                cancellationToken);
        }


        await _healthCheckResponseRepository.SaveAsync(record, cancellationToken);

        return record;
    }
}
