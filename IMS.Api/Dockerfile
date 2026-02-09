# See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

# This stage is used when running from VS in fast mode (Default for Debug configuration)
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
RUN apt-get update && apt-get install -y curl
WORKDIR /app
EXPOSE 8080
EXPOSE 8081


# This stage is used to build the service project
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["Directory.Packages.props", "."]
COPY ["IMS.Api/IMS.Api.csproj", "IMS.Api/"]
COPY ["IMS.Infrastructure/IMS.Infrastructure.csproj", "IMS.Infrastructure/"]
COPY ["IMS.Application/IMS.Application.csproj", "IMS.Application/"]
COPY ["IMS.Core/IMS.Core.csproj", "IMS.Core/"]
RUN dotnet restore "./IMS.Api/IMS.Api.csproj"
COPY . .
WORKDIR "/src/IMS.Api"
RUN dotnet build "./IMS.Api.csproj" -c $BUILD_CONFIGURATION -o /app/build

# This stage is used to publish the service project to be copied to the final stage
FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./IMS.Api.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

# This stage is used in production or when running from VS in regular mode (Default when not using the Debug configuration)
FROM base AS final
WORKDIR /app

COPY --from=publish /app/publish .
COPY IMS.Api/appsettings.json /app/appsettings.json
COPY IMS.Api/appsettings.Development.json /app/appsettings.Development.json

ENTRYPOINT ["dotnet", "IMS.Api.dll"]
