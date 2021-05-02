#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:5.0 AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:5.0 AS build
WORKDIR /src
COPY ["src/BGG.Bot/BGG.Bot.csproj", "src/BGG.Bot/"]
COPY ["src/BGG.Bot.Core/BGG.Bot.Core.csproj", "src/BGG.Bot.Core/"]
COPY ["src/BGG.Bot.Data/BGG.Bot.Data.csproj", "src/BGG.Bot.Data/"]
RUN dotnet restore "src/BGG.Bot/BGG.Bot.csproj"
COPY . .
WORKDIR "/src/src/BGG.Bot"
RUN dotnet build "BGG.Bot.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "BGG.Bot.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "BGG.Bot.dll"]
