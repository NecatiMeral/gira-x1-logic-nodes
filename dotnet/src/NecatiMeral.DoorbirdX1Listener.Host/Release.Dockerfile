FROM mcr.microsoft.com/dotnet/aspnet:6.0-bullseye-slim AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/sdk:6.0-bullseye-slim AS build
WORKDIR /src
COPY ["src/NecatiMeral.DoorbirdX1Listener.Host/NecatiMeral.DoorbirdX1Listener.Host.csproj", "src/NecatiMeral.DoorbirdX1Listener.Host/"]
COPY ["*.props", "*.targets", "./"]
RUN dotnet restore "src/NecatiMeral.DoorbirdX1Listener.Host/NecatiMeral.DoorbirdX1Listener.Host.csproj"
COPY . .
WORKDIR "/src/src/NecatiMeral.DoorbirdX1Listener.Host"
RUN dotnet build "NecatiMeral.DoorbirdX1Listener.Host.csproj" --no-restore -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "NecatiMeral.DoorbirdX1Listener.Host.csproj" --no-build -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "NecatiMeral.DoorbirdX1Listener.Host.dll"]
