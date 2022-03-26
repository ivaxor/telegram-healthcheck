using System.ComponentModel.DataAnnotations;

namespace IVAXOR.TelegramHealthCheck.Models;

public class CosmosDbConfiguration
{
    /// <summary>
    /// Connection string
    /// </summary>
    [Required]
    public string ConnectionString { get; set; }
    
    /// <summary>
    /// Database name
    /// </summary>
    [Required]
    public string Database { get; set; }
}
