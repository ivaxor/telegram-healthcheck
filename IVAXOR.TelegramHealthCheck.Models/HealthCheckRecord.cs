using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;

namespace IVAXOR.TelegramHealthCheck.Models;

public class HealthCheckRecord
{
    /// <summary>
    /// Unique identifier
    /// </summary>
    [Required]
    [JsonProperty(PropertyName = "id")]
    public string Id { get; set; }

    /// <summary>
    /// Checked at
    /// </summary>
    [Required]
    [JsonProperty(PropertyName = "checkedAt")]
    public DateTime CheckedAt { get; set; }

    /// <summary>
    /// Status code
    /// </summary>
    [Required]
    [JsonProperty(PropertyName = "statusCode")]
    public int StatusCode { get; set; }

    /// <summary>
    /// Status text
    /// </summary>
    [Required]
    [JsonProperty(PropertyName = "statusText")]
    public string StatusText { get; set; }

    public HealthCheckRecord() { }

    public HealthCheckRecord(string id, int statusCode, string statusText)
    {
        Id = id;
        CheckedAt = DateTime.UtcNow;
        StatusCode = statusCode;
        StatusText = statusText;
    }

    public HealthCheckRecord(string id, HealthCheckResponse healthCheckResponse)
        : this(id, healthCheckResponse.StatusCode, healthCheckResponse.StatusText) { }

    public override string ToString()
    {
        return JsonConvert.SerializeObject(this);
    }
}
