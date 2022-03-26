using IVAXOR.TelegramHealthCheck.Infrastructure.Startup;
using Microsoft.Azure.Functions.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection;

[assembly: FunctionsStartup(typeof(IVAXOR.TelegramHealthCheck.Function.Startup))]
namespace IVAXOR.TelegramHealthCheck.Function;

public class Startup : FunctionsStartup
{
    public override async void Configure(IFunctionsHostBuilder builder)
    {
        builder.Services.AddHttpClient();

        ServicesStartup.Add(builder.Services);
        await CosmosDbStartup.AddAsync(builder.GetContext().Configuration, builder.Services);
    }
}
