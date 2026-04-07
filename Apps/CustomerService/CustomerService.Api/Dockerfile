# Stage 1: Build the application
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Copy all .csproj files first to restore dependencies (Optimizes Docker Cache)
COPY ["MasterService/MasterService.Api/MasterService.Api.csproj", "MasterService/MasterService.Api/"]
COPY ["MasterService/MasterService.Application/MasterService.Application.csproj", "MasterService/MasterService.Application/"]
COPY ["MasterService/MasterService.Domain/MasterService.Domain.csproj", "MasterService/MasterService.Domain/"]
COPY ["MasterService/MasterService.Infrastructure/MasterService.Infrastructure.csproj", "MasterService/MasterService.Infrastructure/"]

# Restore all projects
RUN dotnet restore "MasterService/MasterService.Api/MasterService.Api.csproj"

# Copy the rest of the source code
COPY . .

# Build the Api project
WORKDIR "/src/MasterService/MasterService.Api"
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