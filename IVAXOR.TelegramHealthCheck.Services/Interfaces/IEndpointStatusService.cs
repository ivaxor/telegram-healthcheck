using IVAXOR.TelegramHealthCheck.Models;

namespace IVAXOR.TelegramHealthCheck.Services.Interfaces;

public interface IEndpointStatusService
{
    public Task<HealthCheckResponse> CheckAsync(string url, CancellationToken cancellationToken = default);
}

