FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src
COPY EmailService/EmailService.csproj EmailService/
RUN dotnet restore EmailService/EmailService.csproj
COPY . .
RUN dotnet publish EmailService/EmailService.csproj -c Release -o /app/publish --no-restore
FROM mcr.microsoft.com/dotnet/runtime:10.0
WORKDIR /app
RUN apt-get update \
    && apt-get install -y --no-install-recommends ca-certificates \
    && update-ca-certificates \
    && rm -rf /var/lib/apt/lists/*
COPY --from=build /app/publish .
ENTRYPOINT ["dotnet", "EmailService.dll"]
