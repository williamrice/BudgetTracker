#!/bin/bash

docker compose up -d budgettracker-db pgadmin

# Wait for the database to be ready
echo "Waiting for database to be ready..."
until docker exec budgettracker-db pg_isready -U postgres; do
  sleep 1
done
echo "Database is ready."

cd BudgetTracker.Web
dotnet watch run