# Build Stage
FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src
COPY ["Services/Ordering/Ordering.API/Ordering.API.csproj", "Services/Ordering/Ordering.API/Ordering.API.csproj"]
COPY ["Services/Ordering/Ordering.Application/Ordering.Application.csproj", "Services/Ordering/Ordering.Application/Ordering.Application.csproj"]
COPY ["Services/Ordering/Ordering.Infrastructure/Ordering.Infrastructure.csproj", "Services/Ordering/Ordering.Infrastructure/Ordering.Infrastructure.csproj"]
COPY ["Services/Ordering/Ordering.Domain/Ordering.Domain.csproj", "Services/Ordering/Ordering.Domain/Ordering.Domain.csproj"]
COPY ["BuildingBlocks/BuildingBlock/BuildingBlock.csproj", "BuildingBlocks/BuildingBlock/BuildingBlock.csproj"]
COPY . .
RUN dotnet restore "Services/Ordering/Ordering.API/Ordering.API.csproj" --disable-parallel
WORKDIR "/src/Services/Ordering/Ordering.API"
RUN dotnet publish "./Ordering.API.csproj" -c release -o /app --no-restore

# Serve Stage
FROM mcr.microsoft.com/dotnet/aspnet:10.0
WORKDIR /app
COPY --from=build /app ./

EXPOSE 8000

ENTRYPOINT ["dotnet", "Ordering.API.dll"]