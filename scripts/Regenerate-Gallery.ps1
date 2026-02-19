#!/usr/bin/env pwsh
<#
.SYNOPSIS
    Regenerates all SVG files for the gallery.

.DESCRIPTION
    Finds all *.stf files in the gallery folder and runs steffi.exe
    to generate the corresponding *.svg files.

.PARAMETER SteffiBin
    Path to the steffi.exe binary. Defaults to the CLI project's bin folder.

.EXAMPLE
    .\Regenerate-Gallery.ps1
    Regenerates all gallery SVGs using the default steffi.exe location.

.EXAMPLE
    .\Regenerate-Gallery.ps1 -SteffiBin "C:\custom\path\steffi.exe"
    Regenerates using a custom steffi.exe location.
#>

param(
    [string]$SteffiBin = "..\src\Steffi.Cli\bin\Debug\net10.0\win-x64\steffi.exe"
)

$ErrorActionPreference = "Stop"

# Resolve paths
$scriptDir = Split-Path -Parent $MyInvocation.MyCommand.Path
$galleryDir = Join-Path $scriptDir "..\gallery" | Resolve-Path
$steffiExe = Join-Path $scriptDir $SteffiBin | Resolve-Path

Write-Host "Regenerating gallery SVGs..." -ForegroundColor Cyan
Write-Host "Using steffi.exe: $steffiExe" -ForegroundColor Gray
Write-Host "Gallery directory: $galleryDir" -ForegroundColor Gray
Write-Host ""

# Find all .stf files
$stfFiles = Get-ChildItem -Path $galleryDir -Filter "*.stf" | Sort-Object Name

if ($stfFiles.Count -eq 0) {
    Write-Host "No .stf files found in $galleryDir" -ForegroundColor Yellow
    exit 0
}

Write-Host "Found $($stfFiles.Count) gallery files" -ForegroundColor Green
Write-Host ""

$successCount = 0
$failCount = 0
$failedFiles = @()

foreach ($stfFile in $stfFiles) {
    $baseName = [System.IO.Path]::GetFileNameWithoutExtension($stfFile.Name)
    $generatedFile = Join-Path $galleryDir "$baseName.svg"

    Write-Host "Processing $baseName..." -NoNewline

    try {
        $output = & $steffiExe generate --input-file $stfFile.FullName --format svg 2>&1

        if ($LASTEXITCODE -ne 0) {
            Write-Host " FAILED" -ForegroundColor Red
            Write-Host "  Error: $output" -ForegroundColor Red
            $failCount++
            $failedFiles += $baseName
        } elseif (Test-Path $generatedFile) {
            Write-Host " OK" -ForegroundColor Green
            $successCount++
        } else {
            Write-Host " FAILED" -ForegroundColor Red
            Write-Host "  Error: Generated file not found: $generatedFile" -ForegroundColor Red
            $failCount++
            $failedFiles += $baseName
        }
    } catch {
        Write-Host " ERROR" -ForegroundColor Red
        Write-Host "  Exception: $($_.Exception.Message)" -ForegroundColor Red
        $failCount++
        $failedFiles += $baseName
    }
}

Write-Host ""
Write-Host "Summary:" -ForegroundColor Cyan
Write-Host "  Success: $successCount" -ForegroundColor Green
Write-Host "  Failed:  $failCount" -ForegroundColor $(if ($failCount -eq 0) { "Green" } else { "Red" })

if ($failedFiles.Count -gt 0) {
    Write-Host ""
    Write-Host "Failed files:" -ForegroundColor Yellow
    foreach ($file in $failedFiles) {
        Write-Host "  - $file" -ForegroundColor Yellow
    }
    exit 1
}

Write-Host ""
Write-Host "All gallery SVGs regenerated successfully!" -ForegroundColor Green
exit 0
