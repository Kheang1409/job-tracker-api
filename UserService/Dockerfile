FROM mcr.microsoft.com/dotnet/sdk:latest AS build
WORKDIR /src

# Copy csproj files and restore dependencies
COPY UserService/UserService.API/UserService.API.csproj ./UserService/UserService.API/
COPY UserService/UserService.Application/UserService.Application.csproj ./UserService/UserService.Application/
COPY UserService/UserService.Domain/UserService.Domain.csproj ./UserService/UserService.Domain/
COPY UserService/UserService.Infrastructure/UserService.Infrastructure.csproj ./UserService/UserService.Infrastructure/

RUN dotnet restore ./UserService/UserService.API/UserService.API.csproj

# Copy full source and build
COPY . .
RUN dotnet publish ./UserService/UserService.API/UserService.API.csproj -c Release -o /app/publish

# Runtime image
FROM mcr.microsoft.com/dotnet/aspnet:latest
WORKDIR /app
COPY --from=build /app/publish .

EXPOSE 5001
ENTRYPOINT ["dotnet", "UserService.API.dll"]
