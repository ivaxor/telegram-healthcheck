namespace IVAXOR.TelegramHealthCheck.Services.Interfaces;

public interface ITelegramService
{
    public Task SendMessageAsync(
        string chatId,
        string botApiKey,
        string message,
        CancellationToken cancellationToken = default);
}
