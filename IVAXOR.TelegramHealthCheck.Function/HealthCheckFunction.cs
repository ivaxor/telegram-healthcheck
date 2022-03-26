using System.Threading.Tasks;
using IVAXOR.TelegramHealthCheck.Services.Interfaces;
using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Logging;

namespace IVAXOR.TelegramHealthCheck.Function;

public class HealthCheckFunction
{
    private readonly IHealthCheckService _healthCheckService;

    public HealthCheckFunction(IHealthCheckService healthCheckService)
    {
        _healthCheckService = healthCheckService;
    }

    [FunctionName("HealthCheck")]
    public async Task Run(
        [TimerTrigger("0 */5 * * * *")] TimerInfo timer,
        ILogger log)
    {
        await _healthCheckService.UpdateAsync();
    }
}
