FROM mcr.microsoft.com/dotnet/sdk:latest AS build
WORKDIR /src
COPY . .
RUN dotnet publish "ApiGateway.csproj" -c Release -o /app/publish

FROM mcr.microsoft.com/dotnet/aspnet:latest
WORKDIR /app
COPY --from=build /app/publish .

EXPOSE 5000

ENTRYPOINT ["dotnet", "ApiGateway.dll"]
