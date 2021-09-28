#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/runtime:3.1 AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:3.1 AS build
WORKDIR /art
COPY ["src/FluxoDeCaixa.Politicas.Workers/FluxoDeCaixa.Politicas.Workers.csproj", "src/FluxoDeCaixa.Politicas.Workers/"]
COPY ["src/FluxoDeCaixa.Infrastructure/FluxoDeCaixa.Infrastructure.csproj", "src/FluxoDeCaixa.Infrastructure/"]
COPY ["src/FluxoDeCaixa.Consolidacao.Application/FluxoDeCaixa.Consolidacao.Application.csproj", "src/FluxoDeCaixa.Consolidacao.Application/"]
COPY ["src/FluxoDeCaixa.Domain/FluxoDeCaixa.Domain.csproj", "src/FluxoDeCaixa.Domain/"]
COPY ["src/System.Extensions/System.Extensions.csproj", "src/System.Extensions/"]
COPY ["src/FluxoDeCaixa.MongoDB/FluxoDeCaixa.MongoDB.csproj", "src/FluxoDeCaixa.MongoDB/"]
COPY ["src/FluxoDeCaixa.Consultas.Application/FluxoDeCaixa.Consultas.Application.csproj", "src/FluxoDeCaixa.Consultas.Application/"]
COPY ["src/FluxoDeCaixa.Lancamentos.Application/FluxoDeCaixa.Lancamentos.Application.csproj", "src/FluxoDeCaixa.Lancamentos.Application/"]
COPY ["src/FluxoDeCaixa.EntityFrameworkCore/FluxoDeCaixa.EntityFrameworkCore.csproj", "src/FluxoDeCaixa.EntityFrameworkCore/"]
COPY ["src/FluxoDeCaixa.Consolidacao.Application.Core/FluxoDeCaixa.Consolidacao.Application.Core.csproj", "src/FluxoDeCaixa.Consolidacao.Application.Core/"]
COPY ["src/MediatR.Extensions.Microsoft.DependencyInjection/MediatR.Extensions.Microsoft.DependencyInjection.csproj", "src/MediatR.Extensions.Microsoft.DependencyInjection/"]
COPY ["src/FluxoDeCaixa.Lancamentos.Application.Core/FluxoDeCaixa.Lancamentos.Application.Core.csproj", "src/FluxoDeCaixa.Lancamentos.Application.Core/"]
COPY ["src/FluxoDeCaixa.RabbitMQ/FluxoDeCaixa.RabbitMQ.csproj", "src/FluxoDeCaixa.RabbitMQ/"]
RUN dotnet restore "src/FluxoDeCaixa.Politicas.Workers/FluxoDeCaixa.Politicas.Workers.csproj"
COPY . .
WORKDIR "/art/src/FluxoDeCaixa.Politicas.Workers"
RUN dotnet build "FluxoDeCaixa.Politicas.Workers.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "FluxoDeCaixa.Politicas.Workers.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "FluxoDeCaixa.Politicas.Workers.dll"]