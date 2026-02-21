FROM mcr.microsoft.com/dotnet/runtime:10.0 AS base
USER $APP_UID
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["PatriotIndex.Ingestion/PatriotIndex.Ingestion.csproj", "PatriotIndex.Ingestion/"]
RUN dotnet restore "PatriotIndex.Ingestion/PatriotIndex.Ingestion.csproj"
COPY . .
WORKDIR "/src/PatriotIndex.Ingestion"
RUN dotnet build "./PatriotIndex.Ingestion.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./PatriotIndex.Ingestion.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "PatriotIndex.Ingestion.dll"]
