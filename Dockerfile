# Použijeme oficiální runtime obraz pro .NET 8
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 80

# Použijeme oficiální SDK obraz pro .NET 8 pro build
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY . .

# Build projektu
RUN dotnet publish "YouTubeDownloader.csproj" -c Release -o /app/out

# Přesuneme build do finálního runtime prostředí
FROM base AS final
WORKDIR /app
COPY --from=build /app/out .
ENTRYPOINT ["dotnet", "YouTubeDownloader.dll"]
