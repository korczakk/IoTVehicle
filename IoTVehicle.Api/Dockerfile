#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/core/aspnet:3.1-buster-slim AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/core/sdk:3.1-buster AS build
WORKDIR /src
COPY ["IoTVehicle.Api/IoTVehicle.Api.csproj", "IoTVehicle.Api/"]
COPY ["MotorDriver/MotorDriver.csproj", "MotorDriver/"]
COPY ["IoT.Shared/IoT.Shared.csproj", "IoT.Shared/"]
COPY ["DistanceSensorDriver/DistanceSensorDriver.csproj", "DistanceSensorDriver/"]
RUN dotnet restore "IoTVehicle.Api/IoTVehicle.Api.csproj"
COPY . .
WORKDIR "/src/IoTVehicle.Api"
RUN dotnet build "IoTVehicle.Api.csproj" -c Release -r linux-arm -o /app/build

FROM build AS publish
RUN dotnet publish "IoTVehicle.Api.csproj" -c Release -r linux-arm -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "IoTVehicle.Api.dll"]