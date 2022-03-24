using System.Net;
using System.Net.Sockets;
using IVAXOR.TelegramHealthCheck.Models;
using IVAXOR.TelegramHealthCheck.Services.Interfaces;
using Microsoft.Extensions.Logging;

namespace IVAXOR.TelegramHealthCheck.Services.Implementations;

public class EndpointStatusService : IEndpointStatusService
{
    private readonly ILogger _logger;
    private readonly HttpClient _httpClient;

    public EndpointStatusService(
        ILogger<EndpointStatusService> logger,
        HttpClient httpClient)
    {
        _logger = logger;
        _httpClient = httpClient;
    }

    public async Task<HealthCheckResponse> CheckAsync(string url, CancellationToken cancellationToken = default)
    {
        _logger.LogInformation("Checking endpoint");

        try
        {
            var httpResponse = await _httpClient.GetAsync(url, cancellationToken);
            return new HealthCheckResponse(httpResponse.StatusCode);
        }
        catch (HttpRequestException exception) when (exception.InnerException is SocketException innerException)
        {
            return new HealthCheckResponse(innerException.SocketErrorCode);
        }
        catch (TaskCanceledException exception) when (exception.InnerException is TimeoutException)
        {
            return new HealthCheckResponse(HttpStatusCode.RequestTimeout);
        }
        finally
        {
            _logger.LogInformation("Endpoint checked");
        }
    }
}
