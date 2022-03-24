using IVAXOR.TelegramHealthCheck.Models;

namespace IVAXOR.TelegramHealthCheck.Services.Interfaces;

public interface IHealthCheckService
{
    public string[] GetIds();

    public Task<HealthCheckRecord[]> UpdateAsync(CancellationToken cancellationToken = default);
    public Task<HealthCheckRecord> UpdateAsync(string id, CancellationToken cancellationToken = default);
}
