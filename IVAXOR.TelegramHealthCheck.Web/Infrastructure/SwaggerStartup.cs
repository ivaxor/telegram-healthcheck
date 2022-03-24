using System.Reflection;
using Microsoft.OpenApi.Models;

namespace IVAXOR.TelegramHealthCheck.Web.Infrastructure;

internal static class SwaggerStartup
{
    public static void AddSwagger(this WebApplicationBuilder builder)
    {
        var version = GetVersion();

        var openApiInfo = new OpenApiInfo()
        {
            Version = version,
            Title = "Telegram HealthCheck",
            Description = "REST web application that provides health check functionality with output to Telegram messenger",
            License = new OpenApiLicense()
            {
                Name = "MIT",
                Url = new Uri("https://raw.githubusercontent.com/ivaxor/telegram-healthcheck/master/LICENSE")
            },
            Contact = new OpenApiContact()
            {
                Name = "IVAXOR",
                Url = new Uri("https://ivaxor.com"),
                Email = "administrator@ivaxor.com"
            }
        };

        builder.Services.AddSwaggerGen(c =>
        {
            c.SwaggerDoc(version, openApiInfo);
        });
    }

    public static void AddSwagger(this WebApplication app)
    {
        var version = GetVersion();

        app.UseSwagger();
        app.UseSwaggerUI(c =>
        {
            c.DocumentTitle = "Telegram HealthCheck";
            c.SwaggerEndpoint($"/swagger/{version}/swagger.json", "Telegram HealthCheck");
        });
    }

    private static string GetVersion()
    {
        return Assembly.GetExecutingAssembly().GetName().Version.ToString();
    }
}
