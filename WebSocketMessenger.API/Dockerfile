#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["WebSocketMessenger.API/WebSocketMessenger.API.csproj", "WebSocketMessenger.API/"]
COPY ["WebSocketMessenger.Core/WebSocketMessenger.Core.csproj", "WebSocketMessenger.Core/"]
COPY ["WebSocketMessenger.Infrastructure/WebSocketMessenger.Infrastructure.csproj", "WebSocketMessenger.Infrastructure/"]
COPY ["WebSocketMessenger.UseCases/WebSocketMessenger.UseCases.csproj", "WebSocketMessenger.UseCases/"]
RUN dotnet restore "WebSocketMessenger.API/WebSocketMessenger.API.csproj"
COPY . .
WORKDIR "/src/WebSocketMessenger.API"
RUN dotnet build "WebSocketMessenger.API.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "WebSocketMessenger.API.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "WebSocketMessenger.API.dll"]