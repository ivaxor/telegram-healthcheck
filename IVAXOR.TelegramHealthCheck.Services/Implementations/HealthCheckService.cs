using IVAXOR.TelegramHealthCheck.Models;
using IVAXOR.TelegramHealthCheck.Models.Constants;
using IVAXOR.TelegramHealthCheck.Services.Interfaces;

namespace IVAXOR.TelegramHealthCheck.Services.Implementations;

public class HealthCheckService : IHealthCheckService
{
    private readonly IEndpointStatusService _endpointStatusService;
    private readonly IHealthCheckConfigurationRepository _healthCheckConfigurationRepository;
    private readonly IHealthCheckRecordRepository _healthCheckRecordRepository;
    private readonly ITelegramService _telegramService;

    public HealthCheckService(
        IEndpointStatusService endpointStatusService,
        IHealthCheckConfigurationRepository healthCheckConfigurationRepository,
        IHealthCheckRecordRepository healthCheckRecordRepository,
        ITelegramService telegramService)
    {
        _endpointStatusService = endpointStatusService;
        _healthCheckConfigurationRepository = healthCheckConfigurationRepository;
        _healthCheckRecordRepository = healthCheckRecordRepository;
        _telegramService = telegramService;
    }

    public async Task<HealthCheckRecord[]> UpdateAsync(CancellationToken cancellationToken = default)
    {
        var configurations = await _healthCheckConfigurationRepository.GetAsync(cancellationToken);
        return await configurations
            .ToAsyncEnumerable()
            .SelectAwait(async _ => await UpdateAsync(_, cancellationToken))
            .ToArrayAsync(cancellationToken);
    }

    public async Task<HealthCheckRecord> UpdateAsync(string id, CancellationToken cancellationToken = default)
    {
        var configuration = await _healthCheckConfigurationRepository.GetAsync(id, cancellationToken);
        return await UpdateAsync(configuration, cancellationToken);
    }

    public async Task<HealthCheckRecord> UpdateAsync(
        HealthCheckConfiguration configuration,
        CancellationToken cancellationToken = default)
    {
        var response = await _endpointStatusService.CheckAsync(configuration.Url, cancellationToken);

        var record = new HealthCheckRecord(configuration.Id, response);
        var previousRecord = await _healthCheckRecordRepository.GetAsync(configuration.Id, cancellationToken);

        var isStatusChanged = previousRecord == null ||
                              previousRecord.StatusCode != record.StatusCode ||
                              record.CheckedAt - previousRecord.CheckedAt > configuration.SendMessageAfterDelayMoreThen;
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


        await _healthCheckRecordRepository.SaveAsync(record, cancellationToken);

        return record;
    }
}
