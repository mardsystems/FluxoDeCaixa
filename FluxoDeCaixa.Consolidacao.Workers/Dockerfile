#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/core/runtime:3.1-buster-slim AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/core/sdk:3.1-buster AS build
WORKDIR /src
COPY ["FluxoDeCaixa.Consolidacao.Workers/FluxoDeCaixa.Consolidacao.Workers.csproj", "FluxoDeCaixa.Consolidacao.Workers/"]
RUN dotnet restore "FluxoDeCaixa.Consolidacao.Workers/FluxoDeCaixa.Consolidacao.Workers.csproj"
COPY . .
WORKDIR "/src/FluxoDeCaixa.Consolidacao.Workers"
RUN dotnet build "FluxoDeCaixa.Consolidacao.Workers.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "FluxoDeCaixa.Consolidacao.Workers.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "FluxoDeCaixa.Consolidacao.Workers.dll"]