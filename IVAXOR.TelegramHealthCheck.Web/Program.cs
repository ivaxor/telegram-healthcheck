using IVAXOR.TelegramHealthCheck.Infrastructure.Startup;
using IVAXOR.TelegramHealthCheck.Web.Startup;

var builder = WebApplication.CreateBuilder(args);
builder.Logging.AddConsole();
#if DEBUG
builder.Logging.AddDebug();
#endif

builder.Services.AddControllers();
builder.Services.AddRazorPages();
builder.Services.AddHttpClient();
builder.Services.AddHealthChecks();

SwaggerStartup.Add(builder.Services);
ServicesStartup.Add(builder.Services);
await CosmosDbStartup.AddAsync(builder.Configuration, builder.Services);

var app = builder.Build();
if (!app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
    app.UseExceptionHandler("/Error");
}

SwaggerStartup.Add(app);
app.UseHealthChecks("/healthcheck");

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapRazorPages();
app.MapControllers();

app.Run();
