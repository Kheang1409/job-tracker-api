FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src

# Copy csproj files and restore dependencies
COPY UserService/UserService.API/UserService.API.csproj ./UserService/UserService.API/
COPY UserService/UserService.Application/UserService.Application.csproj ./UserService/UserService.Application/
COPY UserService/UserService.Domain/UserService.Domain.csproj ./UserService/UserService.Domain/
COPY UserService/UserService.Infrastructure/UserService.Infrastructure.csproj ./UserService/UserService.Infrastructure/
COPY SharedKernel/SharedKernel.csproj ./SharedKernel/
COPY BuildingBlocks/BuildingBlocks.Domain/BuildingBlocks.Domain.csproj ./BuildingBlocks/BuildingBlocks.Domain/

RUN dotnet restore ./UserService/UserService.API/UserService.API.csproj

# Copy full source and build
COPY . .
RUN dotnet publish ./UserService/UserService.API/UserService.API.csproj -c Release -o /app/publish --no-restore

# Runtime image
FROM mcr.microsoft.com/dotnet/aspnet:10.0
WORKDIR /app
RUN apt-get update \
    && apt-get install -y --no-install-recommends ca-certificates curl \
    && update-ca-certificates \
    && rm -rf /var/lib/apt/lists/*
COPY --from=build /app/publish .

EXPOSE 8080
ENTRYPOINT ["dotnet", "UserService.API.dll"]
