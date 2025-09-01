# Script to set up the development environment

Write-Host "Starting development environment setup..." -ForegroundColor Cyan

# Create secrets and certificate path
$secretsPath = "$env:APPDATA\Microsoft\UserSecrets"
Write-Host "Creating secrets directory at: $secretsPath" -ForegroundColor Yellow
New-Item -ItemType Directory -Path $secretsPath -Force | Out-Null
$certPath = "$env:APPDATA\ASP.NET\Https"
Write-Host "Creating certificate directory at: $certPath" -ForegroundColor Yellow
New-Item -ItemType Directory -Path $certPath -Force | Out-Null

# Initializes user secrets in the project
Write-Host "Initializes user secrets in the project..." -ForegroundColor Yellow
dotnet user-secrets init --project src/Ambev.DeveloperEvaluation.WebApi
Write-Host "Successfully initialized user secrets at: Ambev.DeveloperEvaluation.WebApi.csproj" -ForegroundColor Green

# Stores the password used in the certificate
Write-Host "Stores the password used in the certificate..." -ForegroundColor Yellow
dotnet user-secrets set "Kestrel:Certificates:Development:Password" "ev@luAt10n" --project src/Ambev.DeveloperEvaluation.WebApi
Write-Host "Password generated successfully" -ForegroundColor Green

# Generate HTTPS development certificate
Write-Host "Generating HTTPS development certificate..." -ForegroundColor Yellow
dotnet dev-certs https -ep "$certPath\Ambev.DeveloperEvaluation.WebApi.pfx" -p ev@luAt10n
Write-Host "Certificate generated successfully at: $certPath\Ambev.DeveloperEvaluation.WebApi.pfx" -ForegroundColor Green

# Build and start Docker containers
Write-Host "Building and starting Docker containers..." -ForegroundColor Yellow
docker-compose up -d
Write-Host "Docker containers are running." -ForegroundColor Green


# Install Entity Framework Core tools
Write-Host "Installing Entity Framework Core tools..." -ForegroundColor Yellow
dotnet new tool-manifest
dotnet tool install dotnet-ef
Write-Host "Entity Framework Core tools installed." -ForegroundColor Green

# Apply Entity Framework migrations
Write-Host "Applying Entity Framework migrations to the database..." -ForegroundColor Yellow
dotnet ef database update `
    --project src/Ambev.DeveloperEvaluation.ORM/Ambev.DeveloperEvaluation.ORM.csproj `
    --startup-project src/Ambev.DeveloperEvaluation.WebApi/Ambev.DeveloperEvaluation.WebApi.csproj `
    --context Ambev.DeveloperEvaluation.ORM.DefaultContext
Write-Host "Database migrations applied successfully." -ForegroundColor Green

Write-Host "Development environment setup completed!" -ForegroundColor Cyan
# End of script