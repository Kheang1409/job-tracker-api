#!/bin/bash

# Wait for Kafka to start
echo "Waiting for Kafka to be available on kafka:9092..."

# Loop until Kafka is reachable
while ! nc -z kafka 9092; do
  echo "Kafka not available. Retrying in 5 seconds..."
  sleep 5
done

echo "Kafka is available. Starting Notification Service..."

# Execute the NotificationService
exec dotnet NotificationService.dll
