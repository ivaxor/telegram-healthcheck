using IVAXOR.TelegramHealthCheck.Services.HostedServices;
using IVAXOR.TelegramHealthCheck.Services.Implementations;
using IVAXOR.TelegramHealthCheck.Services.Interfaces;

namespace IVAXOR.TelegramHealthCheck.Web.Infrastructure;

internal static class ServiceStartup
{
    public static void AddServices(this WebApplicationBuilder builder)
    {
        builder.Services.AddSingleton<IHealthCheckConfigurationProvider, HealthCheckConfigurationProvider>();
        builder.Services.AddSingleton<IHealthCheckResponseRepository, CosmosDbHealthCheckRepository>();
        builder.Services.AddSingleton<IEndpointStatusService, EndpointStatusService>();
        builder.Services.AddSingleton<IHealthCheckService, HealthCheckService>();
        builder.Services.AddSingleton<ITelegramService, TelegramService>();

        builder.Services.AddHostedService<HealthCheckHostedService>();
    }
}
