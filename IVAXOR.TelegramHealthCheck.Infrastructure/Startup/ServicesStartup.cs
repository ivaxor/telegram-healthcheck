using IVAXOR.TelegramHealthCheck.Services.Implementations;
using IVAXOR.TelegramHealthCheck.Services.Interfaces;
using Microsoft.Extensions.DependencyInjection;

namespace IVAXOR.TelegramHealthCheck.Infrastructure.Startup;

public static class ServicesStartup
{
    public static void Add(IServiceCollection services)
    {
        services.AddSingleton<IEndpointStatusService, EndpointStatusService>();
        services.AddSingleton<IHealthCheckConfigurationRepository, HealthCheckConfigurationRepository>();
        services.AddSingleton<IHealthCheckRecordRepository, HealthCheckRecordRepository>();
        services.AddSingleton<IHealthCheckService, HealthCheckService>();
        services.AddSingleton<ITelegramService, TelegramService>();
    }
}
