# Použij oficiální .NET image jako základ
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 8080

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY ["YouTubeDownloader.csproj", "./"]
RUN dotnet restore "YouTubeDownloader.csproj"
COPY . .
RUN dotnet publish "YouTubeDownloader.csproj" -c Release -o /app/out

FROM base AS final
WORKDIR /app

# Instalace yt-dlp s automatickou aktualizací
RUN apt-get update && \
    apt-get install -y wget && \
    wget https://github.com/yt-dlp/yt-dlp/releases/latest/download/yt-dlp -O /usr/local/bin/yt-dlp && \
    chmod a+rx /usr/local/bin/yt-dlp

# Příkaz pro automatickou aktualizaci yt-dlp při spuštění
RUN echo '#!/bin/bash\nwget https://github.com/yt-dlp/yt-dlp/releases/latest/download/yt-dlp -O /usr/local/bin/yt-dlp && chmod a+rx /usr/local/bin/yt-dlp\nexec "$@"' > /app/entrypoint.sh && chmod +x /app/entrypoint.sh

COPY --from=build /app/out .
ENTRYPOINT ["/app/entrypoint.sh", "dotnet", "YouTubeDownloader.dll"]
