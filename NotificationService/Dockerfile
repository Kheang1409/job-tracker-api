FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /app

COPY . . 

RUN dotnet publish -c Release -o out

FROM mcr.microsoft.com/dotnet/aspnet:9.0
WORKDIR /app

COPY --from=build /app/out . 
COPY wait-for-kafka.sh /app/wait-for-kafka.sh

RUN apt-get update && \
    apt-get install -y netcat-openbsd && \
    rm -rf /var/lib/apt/lists/*

RUN chmod +x /app/wait-for-kafka.sh && \
    echo "Kafka wait script copied and permissions fixed"

EXPOSE 5001

ENTRYPOINT ["/app/wait-for-kafka.sh"]
