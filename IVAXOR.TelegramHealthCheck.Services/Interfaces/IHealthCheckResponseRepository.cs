using IVAXOR.TelegramHealthCheck.Models;

namespace IVAXOR.TelegramHealthCheck.Services.Interfaces;

public interface IHealthCheckResponseRepository
{
    public Task<HealthCheckRecord[]> GetAsync(CancellationToken cancellationToken = default);
    public Task<HealthCheckRecord?> GetAsync(string id, CancellationToken cancellationToken = default);

    public Task SaveAsync(HealthCheckRecord record, CancellationToken cancellationToken = default);
}
