# Používáme oficiální obraz .NET 7 pro runtime
FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app
EXPOSE 80

# Používáme oficiální obraz .NET 7 SDK pro build
FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY . .

# Build projektu
RUN dotnet publish "YouTubeDownloader.csproj" -c Release -o /app/out

# Přesuneme build do finálního prostředí
FROM base AS final
WORKDIR /app
COPY --from=build /app/out .
ENTRYPOINT ["dotnet", "YouTubeDownloader.dll"]
