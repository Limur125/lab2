FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

COPY ["src/Gateway/Gateway.csproj", "Gateway/"]
COPY ["src/Kernel/Kernel.csproj", "Kernel/"]

RUN dotnet restore "Gateway"
COPY /src .
WORKDIR "/src/Gateway"
RUN dotnet build -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Gateway.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Gateway.dll"]