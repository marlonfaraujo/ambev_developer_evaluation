# Script to set up the development environment

Write-Host "Starting development environment setup..." -ForegroundColor Cyan

# Define certificate path
$certPath = "$env:APPDATA\ASP.NET\Https"
Write-Host "Creating certificate directory at: $certPath" -ForegroundColor Yellow
New-Item -ItemType Directory -Path $certPath -Force | Out-Null

# Generate HTTPS development certificate
Write-Host "Generating HTTPS development certificate..." -ForegroundColor Yellow
dotnet dev-certs https -ep "$certPath\Ambev.DeveloperEvaluation.WebApi.pfx" -p ev@luAt10n
Write-Host "Certificate generated successfully at: $certPath\Ambev.DeveloperEvaluation.WebApi.pfx" -ForegroundColor Green

# Build and start Docker containers
Write-Host "Building and starting Docker containers..." -ForegroundColor Yellow
docker-compose up --build
Write-Host "Docker containers are running." -ForegroundColor Green

# Apply Entity Framework migrations
Write-Host "Applying Entity Framework migrations to the database..." -ForegroundColor Yellow
dotnet ef database update `
    --project src/Ambev.DeveloperEvaluation.ORM/Ambev.DeveloperEvaluation.ORM.csproj `
    --startup-project src/Ambev.DeveloperEvaluation.WebApi/Ambev.DeveloperEvaluation.WebApi.csproj `
    --context Ambev.DeveloperEvaluation.ORM.DefaultContext
Write-Host "Database migrations applied successfully." -ForegroundColor Green

Write-Host "Development environment setup completed!" -ForegroundColor Cyan
# End of script