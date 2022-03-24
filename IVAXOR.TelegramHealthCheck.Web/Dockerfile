#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["IVAXOR.TelegramHealthCheck.Web/IVAXOR.TelegramHealthCheck.Web.csproj", "IVAXOR.TelegramHealthCheck.Web/"]
RUN dotnet restore "IVAXOR.TelegramHealthCheck.Web/IVAXOR.TelegramHealthCheck.Web.csproj"
COPY . .
WORKDIR "/src/IVAXOR.TelegramHealthCheck.Web"
RUN dotnet build "IVAXOR.TelegramHealthCheck.Web.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "IVAXOR.TelegramHealthCheck.Web.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "IVAXOR.TelegramHealthCheck.Web.dll"]