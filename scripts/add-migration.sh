#!/bin/bash

if [ -z "$1" ]; then
  echo "Usage: $0 <MigrationName>"
  exit 1
fi

MIGRATION_NAME=$1

dotnet ef migrations add "$MIGRATION_NAME" \
  --project ./BudgetTracker.Data/BudgetTracker.Data.csproj \
  --startup-project ./BudgetTracker.Web/BudgetTracker.Web.csproj