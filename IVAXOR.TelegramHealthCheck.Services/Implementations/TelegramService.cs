using IVAXOR.TelegramHealthCheck.Services.Interfaces;
using Microsoft.Extensions.Logging;

namespace IVAXOR.TelegramHealthCheck.Services.Implementations;

public class TelegramService : ITelegramService
{
    private readonly ILogger _logger;
    private readonly HttpClient _httpClient;

    public TelegramService(
        ILogger<TelegramService> logger,
        HttpClient httpClient)
    {
        _logger = logger;
        _httpClient = httpClient;
    }

    public async Task SendMessageAsync(
        string chatId,
        string botApiKey,
        string message,
        CancellationToken cancellationToken = default)
    {
        _logger.LogInformation("Sending Telegram message");

        var url = $"https://api.telegram.org/bot{botApiKey}/sendMessage";
        var content = new FormUrlEncodedContent(new[]
        { 
            new KeyValuePair<string, string>("chat_id", chatId),
            new KeyValuePair<string, string>("text", message)
        });

        var response = await _httpClient.PostAsync(url, content, cancellationToken);
        if (response.IsSuccessStatusCode) _logger.LogInformation("Telegram message sent");
        else _logger.LogError(
            "Failed to send Telegram message because {StatusText} ({StatusCode})",
            Enum.GetName(response.StatusCode),
            response.StatusCode);
    }
}
