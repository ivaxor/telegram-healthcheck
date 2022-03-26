using IVAXOR.TelegramHealthCheck.Models;

namespace IVAXOR.TelegramHealthCheck.Services.Interfaces;

public interface IHealthCheckConfigurationRepository
{
    public Task<HealthCheckConfiguration[]> GetAsync(CancellationToken cancellationToken = default);
    public Task<HealthCheckConfiguration?> GetAsync(string id, CancellationToken cancellationToken = default);
}
