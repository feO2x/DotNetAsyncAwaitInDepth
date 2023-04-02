FROM mcr.microsoft.com/dotnet/runtime-deps:7.0-alpine as base
WORKDIR /app
EXPOSE 80
RUN adduser -u 1000 --disabled-password --gecos "" appuser && chown -R appuser /app
USER appuser

FROM mcr.microsoft.com/dotnet/sdk:7.0 as build
WORKDIR /src
COPY ["./AsyncVsSync.Backend.csproj", "./AsyncVsSync.Backend.csproj"]
RUN dotnet restore "./AsyncVsSync.Backend.csproj" --runtime alpine-x64
COPY . .
RUN dotnet publish "./AsyncVsSync.Backend.csproj" \
    -c Release \
    --no-restore \
    --runtime alpine-x64\
    --self-contained true\
    /p:PublishTrimmed=true\
    /p:PublishSingleFile=true\
    -o "../app"

FROM base
WORKDIR /app
COPY --from=build /app .