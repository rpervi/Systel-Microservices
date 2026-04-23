# Stage 1: Build & Restore
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Copy project files for caching optimization
# Note: We still copy Systel.Shared because BFF depends on it
COPY ["Systel.Shared/Systel.Shared.csproj", "Systel.Shared/"]
COPY ["Apps/BFFService/BFFService.Api/BFFService.Api.csproj", "Apps/BFFService/BFFService.Api/"]
COPY ["Apps/BFFService/BFFService.Application/BFFService.Application.csproj", "Apps/BFFService/BFFService.Application/"]
COPY ["Apps/BFFService/BFFService.Infrastructure/BFFService.Infrastructure.csproj", "Apps/BFFService/BFFService.Infrastructure/"]

# Restore dependencies
RUN dotnet restore "Apps/BFFService/BFFService.Api/BFFService.Api.csproj"

# Copy the entire source code
COPY . .

# Build the BFF Api project
WORKDIR "/src/Apps/BFFService/BFFService.Api"
RUN dotnet build "BFFService.Api.csproj" -c Release -o /app/build

# Stage 2: Publish
FROM build AS publish
RUN dotnet publish "BFFService.Api.csproj" -c Release -o /app/publish /p:UseAppHost=false

# Stage 3: Final Runtime
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS final
WORKDIR /app

# .NET 8 Environment setup
ENV ASPNETCORE_URLS=http://+:80
EXPOSE 80

COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "BFFService.Api.dll"]