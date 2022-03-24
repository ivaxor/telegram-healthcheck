using System.ComponentModel.DataAnnotations;

namespace IVAXOR.TelegramHealthCheck.Models.Configurations;

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

    /// <summary>
    /// Table name
    /// </summary>
    [Required]
    public string Table { get; set; }

    /// <summary>
    /// Row info outdated after
    /// </summary>
    [Required]
    public TimeSpan OutdatedAfter { get; set; }
}
