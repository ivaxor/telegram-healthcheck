using IVAXOR.TelegramHealthCheck.Models.Configurations;

namespace IVAXOR.TelegramHealthCheck.Services.Interfaces;

public interface IHealthCheckConfigurationProvider
{
    public string[] GetIds();

    public HealthCheckConfiguration Get(string id);
    public HealthCheckConfiguration[] Get();

    TimeSpan GetOutdatedAfter();
}
