FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /src

# Copy solution and project files to use docker caching
COPY TSMetrics.sln ./
COPY src/Core/Core.csproj src/Core/
COPY src/Infrastructure/Infrastructure.csproj src/Infrastructure/
COPY src/TSMetricsAPI/TSMetricsAPI.csproj src/TSMetricsAPI/

RUN dotnet restore src/TSMetricsAPI/TSMetricsAPI.csproj

COPY src/ ./src/

RUN dotnet publish src/TSMetricsAPI/TSMetricsAPI.csproj -c Release -o /app/publish /p:UseAppHost=false

FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS development
WORKDIR /app
COPY --from=build /app/publish ./

ENV ASPNETCORE_URLS="http://+:5221;https://+:7282"

EXPOSE 5221

ENTRYPOINT ["dotnet", "TSMetricsAPI.dll"]
