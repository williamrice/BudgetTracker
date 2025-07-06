#!/bin/bash
set -e

# Apply EF Core migrations
dotnet ef database update --no-build --project BudgetTracker.Web/BudgetTracker.Web.csproj --startup-project BudgetTracker.Web/BudgetTracker.Web.csproj

# Start the app
exec dotnet BudgetTracker.Web.dll