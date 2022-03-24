using System.Timers;
using IVAXOR.TelegramHealthCheck.Models.Configurations;
using IVAXOR.TelegramHealthCheck.Services.Interfaces;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Timer = System.Timers.Timer;

namespace IVAXOR.TelegramHealthCheck.Services.HostedServices;

public class HealthCheckHostedService : IHostedService
{
    private readonly ILogger _logger;
    private readonly IHealthCheckConfigurationProvider _healthCheckConfigurationProvider;
    private readonly IHealthCheckService _healthCheckService;
    private readonly List<Tuple<Timer, HealthCheckConfiguration[]>> _timers;

    public HealthCheckHostedService(
        ILogger<HealthCheckHostedService> logger,
        IHealthCheckConfigurationProvider healthCheckConfigurationProvider,
        IHealthCheckService healthCheckService)
    {
        _logger = logger;
        _healthCheckConfigurationProvider = healthCheckConfigurationProvider;
        _healthCheckService = healthCheckService;

        _timers = new List<Tuple<Timer, HealthCheckConfiguration[]>>();
    }

    public Task StartAsync(CancellationToken cancellationToken)
    {
        _logger.LogInformation("Starting hosted service");

        var healthCheckConfigurations = _healthCheckConfigurationProvider.Get();
        _timers.Clear();

        var newTimers = healthCheckConfigurations
            .GroupBy(_ => _.UpdateEach.TotalMilliseconds)
            .Select(_ =>
            {
                var timer = new Timer(_.Key);
                var groupedConfigurations = _.ToArray();
                timer.Elapsed += (sender, e) => Timer_Elapsed(sender, e, groupedConfigurations);

                return new Tuple<Timer, HealthCheckConfiguration[]>(timer, groupedConfigurations);
            });
        _timers.AddRange(newTimers);

        _timers.ForEach(_ => _.Item1.Start());

        _logger.LogInformation("Hosted service started");

        return Task.CompletedTask;
    }

    public Task StopAsync(CancellationToken cancellationToken)
    {
        _logger.LogInformation("Stopping hosted service");

        _timers.ForEach(_ => _.Item1.Stop());

        _logger.LogInformation("Hosted service stopped");

        return Task.CompletedTask;
    }

    private async void Timer_Elapsed(object? sender, ElapsedEventArgs e, HealthCheckConfiguration[] configurations)
    {
        _logger.LogInformation("Timer elapsed");

        await configurations
            .ToAsyncEnumerable()
            .SelectAwait(async _ => await _healthCheckService.UpdateAsync(_))
            .ToArrayAsync();
    }
}
