# Build Stage
FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src
COPY ["Services/Catalog/Catalog.API/Catalog.API.csproj", "Services/Catalog/Catalog.API/Catalog.API.csproj"]
COPY ["BuildingBlocks/BuildingBlock/BuildingBlock.csproj", "BuildingBlocks/BuildingBlock/BuildingBlock.csproj"]
COPY . .
RUN dotnet restore "Services/Catalog/Catalog.API/Catalog.API.csproj" --disable-parallel
WORKDIR "/src/Services/Catalog/Catalog.API"
RUN dotnet publish "./Catalog.API.csproj" -c release -o /app --no-restore

# Serve Stage
FROM mcr.microsoft.com/dotnet/aspnet:10.0
WORKDIR /app
COPY --from=build /app ./

EXPOSE 8000

ENTRYPOINT ["dotnet", "Catalog.API.dll"]