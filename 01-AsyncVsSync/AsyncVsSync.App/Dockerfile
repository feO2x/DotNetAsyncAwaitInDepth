FROM mcr.microsoft.com/dotnet/runtime-deps:7.0-alpine as base
# Install cultures (same approach as Alpine SDK image, otherwise Humanizr will throw)
RUN apk add --no-cache icu-libs
# Disable the invariant mode (set in base image)
ENV DOTNET_SYSTEM_GLOBALIZATION_INVARIANT=false
WORKDIR /app
RUN adduser -u 1000 --disabled-password --gecos "" appuser && chown -R appuser /app
USER appuser

FROM mcr.microsoft.com/dotnet/sdk:7.0 as build
WORKDIR /src
COPY ["./AsyncVsSync.App.csproj", "./AsyncVsSync.App.csproj"]
RUN dotnet restore "./AsyncVsSync.App.csproj" --runtime alpine-x64
COPY . .
RUN dotnet publish "./AsyncVsSync.App.csproj" \
    -c Release \
    --no-restore \
    --runtime alpine-x64 \
    --self-contained true \
    /p:PublishSingleFile=true \
    -o "../app"

FROM base
WORKDIR /app
COPY --from=build /app .