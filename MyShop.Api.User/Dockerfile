#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/core/aspnet:3.1-buster-slim AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/core/sdk:3.1-buster AS build
WORKDIR /src
COPY ["MyShop.Api.User/MyShop.User.Api.csproj", "MyShop.Api.User/"]
COPY ["MyShop.Application.User/MyShop.Users.Application.csproj", "MyShop.Application.User/"]
COPY ["MyShop.Entity/MyShop.Domain.csproj", "MyShop.Entity/"]
COPY ["MyShop.Application.Contract/MyShop.Application.Contract.csproj", "MyShop.Application.Contract/"]
COPY ["MyShop.Application.Core/MyShop.Application.Core.csproj", "MyShop.Application.Core/"]
COPY ["MyShop.Users.Application.Contract/MyShop.Users.Application.Contract.csproj", "MyShop.Users.Application.Contract/"]
COPY ["MyShop.EntityFrameworkCore/MyShop.EntityFrameworkCore.csproj", "MyShop.EntityFrameworkCore/"]
COPY ["MyShop.Api.Core/MyShop.Api.Core.csproj", "MyShop.Api.Core/"]
RUN dotnet restore "MyShop.Api.User/MyShop.User.Api.csproj"
COPY . .
WORKDIR "/src/MyShop.Api.User"
RUN dotnet build "MyShop.User.Api.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "MyShop.User.Api.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "MyShop.User.Api.dll"]