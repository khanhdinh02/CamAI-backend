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
COPY ["Infrastructure.Repositories/Infrastructure.Repositories.csproj", "Infrastructure.Repositories/"]
COPY ["Infrastructure.Logging/Infrastructure.Logging.csproj", "Infrastructure.Logging/"]
COPY ["Infrastructure.Mapping/Infrastructure.Mapping.csproj", "Infrastructure.Mapping/"]
RUN dotnet restore "Host.CamAI.API/Host.CamAI.API.csproj"
COPY . .
WORKDIR "/src/Host.CamAI.API"
RUN dotnet build "Host.CamAI.API.csproj" --no-restore -c Release --no-restore -o /app/build 

FROM build AS publish
RUN dotnet publish "Host.CamAI.API.csproj" -c Release --no-restore -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Host.CamAI.API.dll"]