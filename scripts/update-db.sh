#!/bin/bash

dotnet ef database update \
  --project ./BudgetTracker.Data/BudgetTracker.Data.csproj \
  --startup-project ./BudgetTracker.Web/BudgetTracker.Web.csproj