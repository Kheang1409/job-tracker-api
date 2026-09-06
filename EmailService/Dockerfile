FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src
COPY EmailService/EmailService.Worker/EmailService.Worker.csproj EmailService/EmailService.Worker/
RUN dotnet restore EmailService/EmailService.Worker/EmailService.Worker.csproj
COPY . .
RUN dotnet publish EmailService/EmailService.Worker/EmailService.Worker.csproj -c Release -o /app/publish --no-restore
FROM mcr.microsoft.com/dotnet/runtime:10.0
WORKDIR /app
RUN apt-get update \
    && apt-get install -y --no-install-recommends ca-certificates \
    && update-ca-certificates \
    && rm -rf /var/lib/apt/lists/*
COPY --from=build /app/publish .
ENTRYPOINT ["dotnet", "EmailService.Worker.dll"]
