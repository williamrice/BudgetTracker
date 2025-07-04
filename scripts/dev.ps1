# PowerShell equivalent of the bash script
docker compose up -d budgettracker-db pgadmin

# Wait for the database to be ready
Write-Host "Waiting for database to be ready..."
do {
    $result = docker exec budgettracker-db pg_isready -U postgres 2>$null
    if ($LASTEXITCODE -eq 0) {
        break
    }
    Start-Sleep -Seconds 1
} while ($true)

Write-Host "Database is ready."

# Change to web directory and run the application
Set-Location BudgetTracker.Web
dotnet watch run