#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER app
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY ["Directory.Packages.props", "."]
COPY ["Host.CamAI.API/Host.CamAI.API.csproj", "Host.CamAI.API/"]
COPY ["Infrastructure.Jwt/Infrastructure.Jwt.csproj", "Infrastructure.Jwt/"]
COPY ["Core.Application/Core.Application.csproj", "Core.Application/"]
COPY ["Core.Domain/Core.Domain.csproj", "Core.Domain/"]
COPY ["Infrastructure.Mapping/Infrastructure.Mapping.csproj", "Infrastructure.Mapping/"]
COPY ["Infrastructure.MessageQueue/Infrastructure.MessageQueue.csproj", "Infrastructure.MessageQueue/"]
COPY ["Infrastructure.Repositories/Infrastructure.Repositories.csproj", "Infrastructure.Repositories/"]
COPY ["Infrastructure.Logging/Infrastructure.Logging.csproj", "Infrastructure.Logging/"]
RUN dotnet restore "./Host.CamAI.API/./Host.CamAI.API.csproj"
COPY . .
WORKDIR "/src/Host.CamAI.API"
RUN dotnet build "./Host.CamAI.API.csproj" -c Release -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./Host.CamAI.API.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Host.CamAI.API.dll"]