# Use SDK to build the application
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /app

# Copy everything into the build container
COPY . . 

# Publish the application
RUN dotnet publish -c Release -o out

# Use ASP.NET runtime to run the application
FROM mcr.microsoft.com/dotnet/aspnet:8.0
WORKDIR /app

# Copy published output and Kafka wait script
COPY --from=build /app/out . 
COPY wait-for-kafka.sh /app/wait-for-kafka.sh

# Install netcat for checking Kafka availability
RUN apt-get update && \
    apt-get install -y netcat-openbsd && \
    rm -rf /var/lib/apt/lists/*

# Ensure the Kafka wait script is executable
RUN chmod +x /app/wait-for-kafka.sh

# Expose port for NotificationService
EXPOSE 5001

# Use the wait script as entry point
ENTRYPOINT ["/app/wait-for-kafka.sh"]
