#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:5.0 AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/sdk:5.0 AS build
WORKDIR /src
COPY ["OrderReturn.HttpApi.Web/OrderReturn.HttpApi.Web.csproj", "OrderReturn.HttpApi.Web/"]
COPY ["OrderReturn.Application/OrderReturn.Application.csproj", "OrderReturn.Application/"]
COPY ["OrderReturn.Domain/OrderReturn.Domain.csproj", "OrderReturn.Domain/"]
COPY ["OrderReturn.Domain.Shared/OrderReturn.Domain.Shared.csproj", "OrderReturn.Domain.Shared/"]
COPY ["OrderReturn.Application.Contracts/OrderReturn.Application.Contracts.csproj", "OrderReturn.Application.Contracts/"]
COPY ["OrderReturn.EntityFrameworkCore.DbMigrations/OrderReturn.EntityFrameworkCore.DbMigrations.csproj", "OrderReturn.EntityFrameworkCore.DbMigrations/"]
COPY ["OrderReturn.EntityFrameworkCore/OrderReturn.EntityFrameworkCore.csproj", "OrderReturn.EntityFrameworkCore/"]
COPY ["OrderReturn.HttpApi/OrderReturn.HttpApi.csproj", "OrderReturn.HttpApi/"]
RUN dotnet restore "OrderReturn.HttpApi.Web/OrderReturn.HttpApi.Web.csproj"
COPY . .
WORKDIR "/src/OrderReturn.HttpApi.Web"
RUN dotnet build "OrderReturn.HttpApi.Web.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "OrderReturn.HttpApi.Web.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "OrderReturn.HttpApi.Web.dll"]