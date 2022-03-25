# Telegram HealthCheck
REST web application that provides health check functionality with output to Telegram messenger

## Before note:
This application was designed in mind as a fully free hosted service for Azure cloud.

## Requirments:
- [Azure Cosmos DB account](https://azure.microsoft.com/services/cosmos-db/)
- [Azure App Service plan](https://docs.microsoft.com/azure/app-service/overview-hosting-plans)
- [Azure App Service](https://azure.microsoft.com/services/app-service)

## Setup:

### Azure:
1. Create dedicated resource group
2. Create required resources manually or using [included templates](/AzureTemplates)

To host this application completly for free insure that you are using next settings to achive that:
- All: Use the same region across all resources
- Azure Cosmos DB account: Use ***Free Tier Discount***
- Azure App Service plan: Use ***F1 tier*** with ***Linux containers***

### Configuration:
One application instance can have multiple health check endpoints. All endpoints are validated using HTTP GET method.

It's ***NOT RECOMMENDED*** to use `UpdateEach` value less than 2 minutes (`0.00:02:00`) because default `HttpClient.Timeout` are 100 seconds.

Configuration for application can be changed in [application settings file](/IVAXOR.TelegramHealthCheck.Web/appsettings.json) or using environmental variables.
There are examples how to configure application:
1. Application settings file:
``` json
"CosmosDbConfiguration": {
    "ConnectionString": "AccountEndpoint=[AccountEndpoint];AccountKey=[AccountKey];"
  },
"HealthCheckConfiguration": [
  {
    "Id": "Azure",
    "Url": "https://portal.azure.com",
    "UpdateEach": "0.00:02:00",
    "TelegramChatId": "[TelegramChatId]",
    "TelegramBotApiKey": "[TelegramBotApiKey]",
    "MessageWhenSucceeded": "🟢 Connection restored",
    "MessageWhenFailed": "🔴 Connection lost because {StatusText} ({StatusCode})"
  },
  {
    "Id": "Google",
    "Url": "https://google.com",
    "UpdateEach": "0.00:05:00",
    "TelegramChatId": "[TelegramChatId]",
    "TelegramBotApiKey": "[TelegramBotApiKey]",
    "MessageWhenSucceeded": "🟢 Connection restored",
    "MessageWhenFailed": "🔴 Connection lost because {StatusText} ({StatusCode})"
  }
]
```
2. Enviromental variables:
``` json
"CosmosDbConfiguration__ConnectionString": "AccountEndpoint=[AccountEndpoint];AccountKey=[AccountKey];"
"HealthCheckConfiguration__0__Id": "Azure"
"HealthCheckConfiguration__0__Url": "https://portal.azure.com"
"HealthCheckConfiguration__0__UpdateEach": "0.00:02:00"
"HealthCheckConfiguration__0__TelegramChatId": "[TelegramChatId]"
"HealthCheckConfiguration__0__TelegramBotApiKey": "[TelegramBotApiKey]"
"HealthCheckConfiguration__0__MessageWhenSucceeded": "🟢 Connection restored"
"HealthCheckConfiguration__0__MessageWhenFailed": "🔴 Connection lost because {StatusText} ({StatusCode})"
"HealthCheckConfiguration__1__Id": "Azure"
"HealthCheckConfiguration__1__Url": "https://google.com"
"HealthCheckConfiguration__1__UpdateEach": "0.00:05:00"
"HealthCheckConfiguration__1__TelegramChatId": "[TelegramChatId]"
"HealthCheckConfiguration__1__TelegramBotApiKey": "[TelegramBotApiKey]"
"HealthCheckConfiguration__1__MessageWhenSucceeded": "🟢 Connection restored"
"HealthCheckConfiguration__1__MessageWhenFailed": "🔴 Connection lost because {StatusText} ({StatusCode})"
```
