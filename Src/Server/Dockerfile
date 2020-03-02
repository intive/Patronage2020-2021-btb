#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/core/aspnet:3.1-buster-slim AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/core/sdk:3.1-buster AS build
WORKDIR /src
COPY ["Src/Server/Server.csproj", "Src/Server/"]
COPY ["Src/Common/Common.csproj", "Src/Common/"]
COPY ["Src/Persistence/Persistence.csproj", "Src/Persistence/"]
COPY ["Src/Application/Application.csproj", "Src/Application/"]
COPY ["Src/Domain/Domain.csproj", "Src/Domain/"]
COPY ["Src/Infrastructure/Infrastructure.csproj", "Src/Infrastructure/"]
COPY ["Src/Client/Client.csproj", "Src/Client/"]
RUN dotnet restore "Src/Server/Server.csproj"
COPY . .
WORKDIR "/src/Src/Server"
RUN dotnet build "Server.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Server.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "BTB.Server.dll"]
