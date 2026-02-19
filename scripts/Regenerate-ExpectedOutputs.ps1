#!/usr/bin/env pwsh
<#
.SYNOPSIS
    Regenerates all expected SVG output files for rendering tests.

.DESCRIPTION
    Finds all *.input.stf files in the RenderingTests folder and runs steffi.exe
    to generate the corresponding *.expected.svg files.

.PARAMETER SteffiBin
    Path to the steffi.exe binary. Defaults to the CLI project's bin folder.

.EXAMPLE
    .\Regenerate-ExpectedOutputs.ps1
    Regenerates all expected outputs using the default steffi.exe location.

.EXAMPLE
    .\Regenerate-ExpectedOutputs.ps1 -SteffiBin "C:\custom\path\steffi.exe"
    Regenerates using a custom steffi.exe location.
#>

param(
    [string]$SteffiBin = "..\src\Steffi.Cli\bin\Debug\net10.0\win-x64\steffi.exe"
)

$ErrorActionPreference = "Stop"

# Resolve paths
$scriptDir = Split-Path -Parent $MyInvocation.MyCommand.Path
$testDir = Join-Path $scriptDir "..\src\Steffi.UnitTests\RenderingTests" | Resolve-Path
$steffiExe = Join-Path $scriptDir $SteffiBin | Resolve-Path

Write-Host "Regenerating expected SVG outputs..." -ForegroundColor Cyan
Write-Host "Using steffi.exe: $steffiExe" -ForegroundColor Gray
Write-Host "Test directory: $testDir" -ForegroundColor Gray
Write-Host ""

# Find all input files
$inputFiles = Get-ChildItem -Path $testDir -Filter "*.input.stf" | Sort-Object Name

if ($inputFiles.Count -eq 0) {
    Write-Host "No input files found in $testDir" -ForegroundColor Yellow
    exit 0
}

Write-Host "Found $($inputFiles.Count) input files" -ForegroundColor Green
Write-Host ""

$successCount = 0
$failCount = 0
$failedTests = @()

foreach ($inputFile in $inputFiles) {
    $testName = $inputFile.BaseName -replace '\.input$', ''
    $expectedFile = Join-Path $testDir "$testName.expected.svg"
    $generatedFile = Join-Path $testDir "$testName.input.svg"
    
    Write-Host "Processing $testName..." -NoNewline
    
    try {
        # Run steffi.exe with generate command (it writes to .input.svg file)
        $output = & $steffiExe generate --input-file $inputFile.FullName --format svg 2>&1
        
        if ($LASTEXITCODE -ne 0) {
            Write-Host " FAILED" -ForegroundColor Red
            Write-Host "  Error: $output" -ForegroundColor Red
            $failCount++
            $failedTests += $testName
        } elseif (Test-Path $generatedFile) {
            # Copy the generated file to the expected file
            Copy-Item $generatedFile $expectedFile -Force
            Remove-Item $generatedFile
            Write-Host " OK" -ForegroundColor Green
            $successCount++
        } else {
            Write-Host " FAILED" -ForegroundColor Red
            Write-Host "  Error: Generated file not found: $generatedFile" -ForegroundColor Red
            $failCount++
            $failedTests += $testName
        }
    } catch {
        Write-Host " ERROR" -ForegroundColor Red
        Write-Host "  Exception: $($_.Exception.Message)" -ForegroundColor Red
        $failCount++
        $failedTests += $testName
    }
}

Write-Host ""
Write-Host "Summary:" -ForegroundColor Cyan
Write-Host "  Success: $successCount" -ForegroundColor Green
Write-Host "  Failed:  $failCount" -ForegroundColor $(if ($failCount -eq 0) { "Green" } else { "Red" })

if ($failedTests.Count -gt 0) {
    Write-Host ""
    Write-Host "Failed tests:" -ForegroundColor Yellow
    foreach ($test in $failedTests) {
        Write-Host "  - $test" -ForegroundColor Yellow
    }
    exit 1
}

Write-Host ""
Write-Host "All expected outputs regenerated successfully!" -ForegroundColor Green
exit 0
