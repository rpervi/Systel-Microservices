# Stage 1: Build the application
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Copy all .csproj files first to restore dependencies (Optimizes Docker Cache)
COPY ["Apps/MasterService/MasterService.Api/MasterService.Api.csproj", "Apps/MasterService/MasterService.Api/"]
COPY ["Apps/MasterService/MasterService.Application/MasterService.Application.csproj", "Apps/MasterService/MasterService.Application/"]
COPY ["Apps/MasterService/MasterService.Domain/MasterService.Domain.csproj", "Apps/MasterService/MasterService.Domain/"]
COPY ["Apps/MasterService/MasterService.Infrastructure/MasterService.Infrastructure.csproj", "Apps/MasterService/MasterService.Infrastructure/"]

# Restore all projects
RUN dotnet restore "Apps/MasterService/MasterService.Api/MasterService.Api.csproj"

# Copy the rest of the source code
COPY . .

# Build the Api project
WORKDIR "/src/Apps/MasterService/MasterService.Api"
RUN dotnet build "MasterService.Api.csproj" -c Release -o /app/build

# Stage 2: Publish the application
FROM build AS publish
RUN dotnet publish "MasterService.Api.csproj" -c Release -o /app/publish /p:UseAppHost=false

# Stage 3: Final Runtime Image
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS final
WORKDIR /app
EXPOSE 80
EXPOSE 443
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "MasterService.Api.dll"]