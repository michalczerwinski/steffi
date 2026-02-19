#!/usr/bin/env pwsh
<#
.SYNOPSIS
    Publishes the Steffi CLI as a native AOT self-contained executable.

.DESCRIPTION
    Builds and publishes steffi.exe for the specified runtime using native AOT
    compilation. The output is a single, self-contained native binary with no
    .NET runtime dependency.

.PARAMETER Runtime
    Target runtime identifier. Defaults to win-x64.
    Examples: win-x64, linux-x64, osx-x64, osx-arm64

.PARAMETER OutputDir
    Output directory for the published binary. Defaults to the standard
    publish folder inside the CLI project's bin directory.

.EXAMPLE
    .\Publish-Cli.ps1
    Publishes a win-x64 AOT build to the default output directory.

.EXAMPLE
    .\Publish-Cli.ps1 -Runtime linux-x64
    Publishes a linux-x64 AOT build.

.EXAMPLE
    .\Publish-Cli.ps1 -Runtime win-x64 -OutputDir "C:\tools"
    Publishes a win-x64 AOT build to a custom directory.
#>

param(
    [string]$Runtime = "win-x64",
    [string]$OutputDir = ""
)

$ErrorActionPreference = "Stop"

$scriptDir = Split-Path -Parent $MyInvocation.MyCommand.Path
$projectPath = (Join-Path $scriptDir "..\src\Steffi.Cli\Steffi.Cli.csproj" | Resolve-Path).ProviderPath
$projectDir  = Split-Path $projectPath -Parent

if ($OutputDir -eq "") {
    $OutputDir = Join-Path $projectDir "bin\Release\net10.0\$Runtime\publish"
}

Write-Host "Publishing Steffi CLI (native AOT)..." -ForegroundColor Cyan
Write-Host "  Project : $projectPath" -ForegroundColor Gray
Write-Host "  Runtime : $Runtime" -ForegroundColor Gray
Write-Host "  Output  : $OutputDir" -ForegroundColor Gray
Write-Host ""

dotnet publish $projectPath `
    --configuration Release `
    --runtime $Runtime

if ($LASTEXITCODE -ne 0) {
    Write-Host ""
    Write-Host "Publish failed." -ForegroundColor Red
    exit 1
}

$exeName = if ($Runtime.StartsWith("win")) { "steffi.exe" } else { "steffi" }
$exePath = Join-Path $OutputDir $exeName

Write-Host ""
if (Test-Path $exePath) {
    $size = (Get-Item $exePath).Length / 1MB
    Write-Host "Published successfully!" -ForegroundColor Green
    Write-Host "  Binary : $exePath" -ForegroundColor Gray
    Write-Host "  Size   : $([math]::Round($size, 1)) MB" -ForegroundColor Gray
} else {
    Write-Host "Published, but binary not found at expected path: $exePath" -ForegroundColor Yellow
}
