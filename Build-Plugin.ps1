$ErrorActionPreference = 'Stop'

$ModsDir = Join-Path ([Environment]::GetFolderPath('MyDocuments')) 'My Games\Stationeers\mods'

if (-not (Test-Path $ModsDir)) {
    Write-Error "Mods directory not found: $ModsDir. Is Stationeers installed?"
    exit 1
}

$ModDir = Join-Path $ModsDir 'StationeersComposterFix'

dotnet build "$PSScriptRoot\StationeersComposterFix\StationeersComposterFix.csproj" -c Release

New-Item -ItemType Directory -Path $ModDir -Force | Out-Null

Copy-Item "$PSScriptRoot\About" $ModDir -Recurse -Force

$Dll = Join-Path $PSScriptRoot 'StationeersComposterFix\bin\Release\netstandard2.0\StationeersComposterFix.dll'
Copy-Item $Dll $ModDir -Force

Write-Host "Plugin deployed to $ModDir" -ForegroundColor Green
