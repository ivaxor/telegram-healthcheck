using System.ComponentModel.DataAnnotations;
using System.Net;
using System.Net.Sockets;

namespace IVAXOR.TelegramHealthCheck.Models;

public class HealthCheckResponse
{

    /// <summary>
    /// Status code
    /// </summary>
    [Required]
    public int StatusCode { get; set; }

    /// <summary>
    /// Status text
    /// </summary>
    [Required]
    public string StatusText { get; set; }

    public HealthCheckResponse(int statusCode, string statusText)
    {
        StatusCode = statusCode;
        StatusText = statusText;
    }

    public HealthCheckResponse(HttpStatusCode httpStatusCode)
        : this((int)httpStatusCode, Enum.GetName(httpStatusCode)) { }

    public HealthCheckResponse(SocketError socketError)
        : this((int)socketError, Enum.GetName(socketError)) { }
}
