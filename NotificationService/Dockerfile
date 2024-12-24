# Build the application
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /app

# Copy everything into the build container
COPY . . 

# Publish the application
RUN dotnet publish -c Release -o out

# Run the application using the ASP.NET runtime
FROM mcr.microsoft.com/dotnet/aspnet:9.0
WORKDIR /app

# Copy the published output and the Kafka wait script
COPY --from=build /app/out . 
COPY wait-for-kafka.sh /app/wait-for-kafka.sh

# Install netcat to check Kafka availability
RUN apt-get update && \
    apt-get install -y netcat-openbsd && \
    rm -rf /var/lib/apt/lists/*

# Ensure the Kafka wait script is executable
RUN chmod +x /app/wait-for-kafka.sh && \
    echo "Kafka wait script copied and permissions fixed"

EXPOSE 5001

ENTRYPOINT ["/app/wait-for-kafka.sh"]
