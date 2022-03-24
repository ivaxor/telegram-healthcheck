namespace IVAXOR.TelegramHealthCheck.Models.Constants;

public static class CosmosDbConstants
{
    public const string ContainerName = $"{nameof(HealthCheckRecord)}s";
    public static string PartitionKeyValue = nameof(HealthCheckRecord.Id).ToLowerInvariant();
    public static string PartitionKeyPath = $"/{PartitionKeyValue}";
}
