using IVAXOR.TelegramHealthCheck.Models.Configurations;
using IVAXOR.TelegramHealthCheck.Services.Interfaces;
using Microsoft.Extensions.Configuration;

namespace IVAXOR.TelegramHealthCheck.Services.Implementations;

public class HealthCheckConfigurationProvider : IHealthCheckConfigurationProvider
{
    private readonly IConfiguration _configuration;

    public HealthCheckConfigurationProvider(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public string[] GetIds()
    {
        return Get()
            .Select(_ => _.Id)
            .ToArray();
    }

    public HealthCheckConfiguration Get(string id)
    {
        return Get()
            .Single(_ => string.Equals(_.Id, id, StringComparison.OrdinalIgnoreCase));
    }

    public HealthCheckConfiguration[] Get()
    {
        return _configuration
            .GetSection(nameof(HealthCheckConfiguration))
            .Get<HealthCheckConfiguration[]>();
    }

    public TimeSpan GetOutdatedAfter()
    {
        return _configuration
            .GetSection(nameof(CosmosDbConfiguration))
            .Get<CosmosDbConfiguration>()
            .OutdatedAfter;
    }
}
