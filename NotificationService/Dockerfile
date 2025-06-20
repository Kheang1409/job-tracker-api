# Runtime image
FROM mcr.microsoft.com/dotnet/aspnet:latest
WORKDIR /app
COPY --from=build /app/publish .

EXPOSE 5003
ENTRYPOINT ["dotnet", "NotificationService.API.dll"]


FROM mcr.microsoft.com/dotnet/sdk:latest AS build
WORKDIR /src

# Copy csproj files and restore dependencies
COPY NotificationService/NotificationService.API/NotificationService.API.csproj ./NotificationService/NotificationService.API/
COPY NotificationService/NotificationService.Application/NotificationService.Application.csproj ./NotificationService/NotificationService.Application/
COPY NotificationService/NotificationService.Domain/NotificationService.Domain.csproj ./NotificationService/NotificationService.Domain/
COPY NotificationService/NotificationService.Infrastructure/NotificationService.Infrastructure.csproj ./NotificationService/NotificationService.Infrastructure/

RUN dotnet restore ./NotificationService/NotificationService.API/NotificationService.API.csproj

# Copy full source and build
COPY . .
RUN dotnet publish ./NotificationService/NotificationService.API/NotificationService.API.csproj -c Release -o /app/publish

# Runtime image
FROM mcr.microsoft.com/dotnet/aspnet:latest
WORKDIR /app
COPY --from=build /app/publish .

EXPOSE 5003
ENTRYPOINT ["dotnet", "NotificationService.API.dll"]
