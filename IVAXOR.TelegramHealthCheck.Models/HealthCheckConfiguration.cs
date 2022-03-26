using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;

namespace IVAXOR.TelegramHealthCheck.Models;

public class HealthCheckConfiguration
{
    /// <summary>
    /// Unique identifier 
    /// </summary>
    [Required]
    [JsonProperty(PropertyName = "id")]
    public string Id { get; set; }

    /// <summary>
    /// Url
    /// </summary>
    [Required]
    [JsonProperty(PropertyName = "url")]
    public string Url { get; set; }
    
    /// <summary>
    /// Send new message if delay is more then
    /// </summary>
    [Required]
    [JsonProperty(PropertyName = "sendMessageAfterDelayMoreThen")]
    public TimeSpan SendMessageAfterDelayMoreThen { get; set; }

    /// <summary>
    /// Telegram chat id
    /// </summary>
    [Required]
    [JsonProperty(PropertyName = "telegramChatId")]
    public string TelegramChatId { get; set; }

    /// <summary>
    /// Telegram bot API key
    /// </summary>
    [Required]
    [JsonProperty(PropertyName = "telegramBotApiKey")]
    public string TelegramBotApiKey { get; set; }

    /// <summary>
    /// Message used when health check probe succeeded
    /// </summary>
    [Required]
    [JsonProperty(PropertyName = "messageWhenSucceeded")]
    public string MessageWhenSucceeded { get; set; }

    /// <summary>
    /// Message used when health check probe failed
    /// {StatusCode} and {StatusText} can be used to insert additional metadata into message
    /// </summary>
    [Required]
    [JsonProperty(PropertyName = "messageWhenFailed")]
    public string MessageWhenFailed { get; set; }

    public override string ToString()
    {
        return JsonConvert.SerializeObject(this);
    }
}
