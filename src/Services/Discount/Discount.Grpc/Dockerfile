# Build Stage
FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src
COPY ["Services/Discount/Discount.Grpc/Discount.Grpc.csproj", "Services/Discount/Discount.Grpc/Discount.Grpc.csproj"]
COPY . .
RUN dotnet restore "Services/Discount/Discount.Grpc/Discount.Grpc.csproj" --disable-parallel
WORKDIR "/src/Services/Discount/Discount.Grpc"
RUN dotnet publish "./Discount.Grpc.csproj" -c release -o /app --no-restore

# Serve Stage
FROM mcr.microsoft.com/dotnet/aspnet:10.0
WORKDIR /app
COPY --from=build /app ./

EXPOSE 8000

ENTRYPOINT ["dotnet", "Discount.Grpc.dll"]