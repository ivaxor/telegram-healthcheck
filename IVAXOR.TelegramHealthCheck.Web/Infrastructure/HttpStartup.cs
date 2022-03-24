namespace IVAXOR.TelegramHealthCheck.Web.Infrastructure;

internal static class HttpStartup
{
    public static void AddHttpClient(this WebApplicationBuilder builder)
    {
        var httpClient = new HttpClient();
        builder.Services.AddSingleton(httpClient);
    }
}
