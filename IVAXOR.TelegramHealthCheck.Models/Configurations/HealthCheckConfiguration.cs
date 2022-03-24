using System.ComponentModel.DataAnnotations;

namespace IVAXOR.TelegramHealthCheck.Models.Configurations;

public class HealthCheckConfiguration
{
    /// <summary>
    /// Unique identifier 
    /// </summary>
    [Required]
    public string Id { get; set; }

    /// <summary>
    /// Url
    /// </summary>
    [Required]
    public string Url { get; set; }

    /// <summary>
    /// Update each
    /// </summary>
    [Required]
    public TimeSpan UpdateEach { get; set; }

    /// <summary>
    /// Telegram chat id
    /// </summary>
    [Required]
    public string TelegramChatId { get; set; }

    /// <summary>
    /// Telegram bot API key
    /// </summary>
    [Required]
    public string TelegramBotApiKey { get; set; }

    /// <summary>
    /// Message used when health check probe succeeded
    /// </summary>
    [Required]
    public string MessageWhenSucceeded { get; set; }

    /// <summary>
    /// Message used when health check probe failed
    /// {StatusCode} and {StatusText} can be used to insert additional metadata into message
    /// </summary>
    [Required]
    public string MessageWhenFailed { get; set; }
}
