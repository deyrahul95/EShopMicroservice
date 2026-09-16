# Build Stage
FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src
COPY ["Services/Basket/Basket.API/Basket.API.csproj", "Services/Basket/Basket.API/Basket.API.csproj"]
COPY ["Services/Basket/Basket.Core/Basket.Core.csproj", "Services/Basket/Basket.Core/Basket.Core.csproj"]
COPY ["BuildingBlocks/BuildingBlock/BuildingBlock.csproj", "BuildingBlocks/BuildingBlock/BuildingBlock.csproj"]
COPY . .
RUN dotnet restore "Services/Basket/Basket.API/Basket.API.csproj" --disable-parallel
WORKDIR "/src/Services/Basket/Basket.API"
RUN dotnet publish "./Basket.API.csproj" -c release -o /app --no-restore

# Serve Stage
FROM mcr.microsoft.com/dotnet/aspnet:10.0
WORKDIR /app
COPY --from=build /app ./

EXPOSE 8000

ENTRYPOINT ["dotnet", "Basket.API.dll"]