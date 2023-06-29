#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/runtime:7.0 AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY ["Twitch.Sample.Service/Twitch.Sample.Service.csproj", "Twitch.Sample.Service/"]
COPY ["Twitch.Sample.Components/Twitch.Sample.Components.csproj", "Twitch.Sample.Components/"]
COPY ["Twitch.Sample.Contracts/Twitch.Sample.Contracts.csproj", "Twitch.Sample.Contracts/"]
COPY ["Twitch.Warehouse.Contracts/Twitch.Warehouse.Contracts.csproj", "Twitch.Warehouse.Contracts/"]
RUN dotnet restore "Twitch.Sample.Service/Twitch.Sample.Service.csproj"
COPY . .
WORKDIR "/src/Twitch.Sample.Service"
RUN dotnet build "Twitch.Sample.Service.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Twitch.Sample.Service.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Twitch.Sample.Service.dll"]



#FROM mcr.microsoft.com/dotnet/sdk:5.0
#
#COPY Twitch.Sample.Service/bin/Release/net5.0/publish/ App/
#WORKDIR /App
#ENTRYPOINT ["dotnet", "Twitch.Sample.Service.dll"]