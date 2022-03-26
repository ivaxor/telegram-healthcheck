namespace IVAXOR.TelegramHealthCheck.Models.Constants;

public static class CosmosDbConstants
{
    public static class Configurations
    {
        public const string ContainerName = $"{nameof(HealthCheckConfiguration)}s";
        public static string PartitionKeyValue = nameof(HealthCheckConfiguration.Id).ToLowerInvariant();
        public static string PartitionKeyPath = $"/{PartitionKeyValue}";
    }

    public static class Records
    {
        public const string ContainerName = $"{nameof(HealthCheckRecord)}s";
        public static string PartitionKeyValue = nameof(HealthCheckRecord.Id).ToLowerInvariant();
        public static string PartitionKeyPath = $"/{PartitionKeyValue}";
    }
}
