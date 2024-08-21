#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER app
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["cryptoworld.news.client/nuget.config", "cryptoworld.news.client/"]
COPY ["CryptoWorld.News.Server/CryptoWorld.Application.Server.csproj", "CryptoWorld.News.Server/"]
COPY ["cryptoworld.news.client/CryptoWorld.News.Client.esproj", "cryptoworld.news.client/"]
COPY ["CryptoWorld.News.Core/CryptoWorld.News.Core.csproj", "CryptoWorld.News.Core/"]
COPY ["CryptoWorld.News.Data/CryptoWorld.News.Data.csproj", "CryptoWorld.News.Data/"]
RUN dotnet restore "./CryptoWorld.News.Server/CryptoWorld.Application.Server.csproj"
COPY . .
WORKDIR "/src/CryptoWorld.News.Server"
RUN dotnet build "./CryptoWorld.Application.Server.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./CryptoWorld.Application.Server.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "CryptoWorld.Application.Server.dll"]