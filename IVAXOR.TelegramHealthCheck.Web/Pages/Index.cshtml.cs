using IVAXOR.TelegramHealthCheck.Models;
using IVAXOR.TelegramHealthCheck.Services.Interfaces;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace IVAXOR.TelegramHealthCheck.Web.Pages
{
    public class IndexModel : PageModel
    {
        private readonly ILogger _logger;
        private readonly IHealthCheckResponseRepository _healthCheckResponseRepository;

        public IEnumerable<HealthCheckRecord> HealthCheckRecords { get; set; }

        public IndexModel(
            ILogger<IndexModel> logger,
            IHealthCheckResponseRepository healthCheckResponseRepository)
        {
            _logger = logger;
            _healthCheckResponseRepository = healthCheckResponseRepository;

            HealthCheckRecords = Enumerable.Empty<HealthCheckRecord>();
        }

        public async Task OnGet(CancellationToken cancellationToken = default)
        {
            _logger.LogInformation("Loading index page");

            HealthCheckRecords = await _healthCheckResponseRepository.GetAsync(cancellationToken);
        }
    }
}