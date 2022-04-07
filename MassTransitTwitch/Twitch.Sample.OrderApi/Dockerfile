#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["Twitch.Sample.OrderApi/Twitch.Sample.OrderApi.csproj", "Twitch.Sample.OrderApi/"]
COPY ["Twitch.Sample.Contracts/Twitch.Sample.Contracts.csproj", "Twitch.Sample.Contracts/"]
RUN dotnet restore "Twitch.Sample.OrderApi/Twitch.Sample.OrderApi.csproj"
COPY . .
WORKDIR "/src/Twitch.Sample.OrderApi"
RUN dotnet build "Twitch.Sample.OrderApi.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Twitch.Sample.OrderApi.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Twitch.Sample.OrderApi.dll"]


#FROM mcr.microsoft.com/dotnet/sdk:5.0
#
#COPY Twitch.Sample.OrderApi/bin/Release/net5.0/publish/ App/
#WORKDIR /App
#ENTRYPOINT ["dotnet", "Twitch.Sample.OrderApi.dll"]