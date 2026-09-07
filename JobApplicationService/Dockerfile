FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src
COPY JobApplicationService/JobApplicationService.csproj ./JobApplicationService/
COPY SharedKernel/SharedKernel.csproj ./SharedKernel/
RUN dotnet restore ./JobApplicationService/JobApplicationService.csproj
COPY . .
RUN dotnet publish ./JobApplicationService/JobApplicationService.csproj -c Release -o /app/publish --no-restore

FROM mcr.microsoft.com/dotnet/aspnet:10.0
WORKDIR /app
RUN apt-get update \
    && apt-get install -y --no-install-recommends ca-certificates curl \
    && update-ca-certificates \
    && rm -rf /var/lib/apt/lists/*
COPY --from=build /app/publish .
EXPOSE 8080
ENTRYPOINT ["dotnet", "JobApplicationService.dll"]
