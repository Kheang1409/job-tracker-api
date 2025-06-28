FROM mcr.microsoft.com/dotnet/sdk:latest AS build
WORKDIR /src

# Copy csproj files and restore dependencies
COPY JobService/JobService.API/JobService.API.csproj ./JobService/JobService.API/
COPY JobService/JobService.Application/JobService.Application.csproj ./JobService/JobService.Application/
COPY JobService/JobService.Domain/JobService.Domain.csproj ./JobService/JobService.Domain/
COPY JobService/JobService.Infrastructure/JobService.Infrastructure.csproj ./JobService/JobService.Infrastructure/

RUN dotnet restore ./JobService/JobService.API/JobService.API.csproj

# Copy full source and build
COPY . .
RUN dotnet publish ./JobService/JobService.API/JobService.API.csproj -c Release -o /app/publish

# Runtime image
FROM mcr.microsoft.com/dotnet/aspnet:latest
WORKDIR /app
COPY --from=build /app/publish .

EXPOSE 5002
ENTRYPOINT ["dotnet", "JobService.API.dll"]

