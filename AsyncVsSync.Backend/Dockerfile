FROM mcr.microsoft.com/dotnet/aspnet:7.0 as base
WORKDIR /app
EXPOSE 80
RUN adduser -u 1000 --disabled-password --gecos "" appuser && chown -R appuser /app
USER appuser

FROM mcr.microsoft.com/dotnet/sdk:7.0 as build
WORKDIR /src
COPY ["./AsyncVsSync.Backend.csproj", "./AsyncVsSync.Backend.csproj"]
RUN dotnet restore "./AsyncVsSync.Backend.csproj"
COPY . .
RUN dotnet publish "./AsyncVsSync.Backend.csproj" -c Release -o "../app" --no-restore 

FROM base
WORKDIR /app
COPY --from=build /app .
ENTRYPOINT ["dotnet", "AsyncVsSync.Backend.dll"]