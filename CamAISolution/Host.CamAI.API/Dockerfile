#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER root
WORKDIR /app
# Install NodeJS
RUN apt-get update && \
    apt-get install -y ca-certificates curl gnupg && \
    mkdir -p /etc/apt/keyrings && \
    curl -fsSL https://deb.nodesource.com/gpgkey/nodesource-repo.gpg.key | gpg --dearmor -o /etc/apt/keyrings/nodesource.gpg
ARG NODE_MAJOR=20
RUN echo "deb [signed-by=/etc/apt/keyrings/nodesource.gpg] https://deb.nodesource.com/node_$NODE_MAJOR.x nodistro main" | tee /etc/apt/sources.list.d/nodesource.list && \
    apt-get update && \
    apt-get install nodejs -y
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
COPY ["Core.Domain/Statics/*.csv", "Core.Domain/Statics/"]
COPY . .
WORKDIR "/src/Host.CamAI.API"
RUN dotnet build "./Host.CamAI.API.csproj" -c Release -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN mkdir -p /app/publish/Core.Domain/Statics && \
    mv /src/Core.Domain/Statics/*.csv /app/publish/Core.Domain/Statics
RUN dotnet publish "./Host.CamAI.API.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
RUN npm install ws
ENTRYPOINT ["dotnet", "Host.CamAI.API.dll"]