﻿FROM mcr.microsoft.com/dotnet/runtime:5.0 AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:5.0 AS build
WORKDIR /src
COPY ["ZebraCorn.csproj", "./"]
RUN dotnet restore "ZebraCorn.csproj"
COPY . .
WORKDIR "/src/"
RUN dotnet build "ZebraCorn.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "ZebraCorn.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "ZebraCorn.dll"]
