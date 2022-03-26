using IVAXOR.TelegramHealthCheck.Models;

namespace IVAXOR.TelegramHealthCheck.Services.Interfaces;

public interface IHealthCheckService
{
    public Task<HealthCheckRecord[]> UpdateAsync(CancellationToken cancellationToken = default);
    public Task<HealthCheckRecord> UpdateAsync(string id, CancellationToken cancellationToken = default);
    public Task<HealthCheckRecord> UpdateAsync(HealthCheckConfiguration configuration, CancellationToken cancellationToken = default);
}
