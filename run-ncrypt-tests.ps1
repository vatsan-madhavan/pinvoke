<#
.Synopsis
    Runs NCrypt Tests
.Parameter Configuration
    The configuration to build. Either "debug" or "release". The default is debug, or the Configuration environment variable if set.
.Parameter WarnAsError
    Converts all build warnings to errors. Useful in preparation to sending a pull request.
#>
[CmdletBinding(SupportsShouldProcess=$true,ConfirmImpact='Medium')]
Param(
    [Parameter()][ValidateSet('debug', 'release')]
    [string]$Configuration = $env:BUILDCONFIGURATION,
    [switch]$WarnAsError = $true
)

$ProjectRoot = Split-Path -parent $PSCommandPath

[System.Management.Automation.Job[]]$jobs = @()
for ($i = 0; $i -lt 200; $i++) {
    $j = Start-Job -ScriptBlock {
        param(
            [string]$ProjectRoot,
            [string]$LogFileName
        )

        if (!$Configuration) { $Configuration = 'debug' }


        # Path variables
        $BinFolder = Join-Path $ProjectRoot "bin"
        $BinConfigFolder = Join-Path $BinFolder $Configuration
        $BinTestsFolder = Join-Path $BinConfigFolder "tests"
        $NCryptTestsFolder = Join-Path $BinTestsFolder "NCrypt.Tests"
        $PackageRestoreRoot = if ($env:NUGET_PACKAGES) { $env:NUGET_PACKAGES } else { Join-Path $env:userprofile '.nuget/packages/' }

        # Set script scope for external tool variables.
        $MSBuildCommand = Get-Command MSBuild.exe -ErrorAction SilentlyContinue

        Function Get-ExternalTools {
            if (($Build -or $Rebuild) -and !$MSBuildCommand) {
                Write-Error "Unable to find MSBuild.exe. Make sure you're running in a VS Developer Prompt."
                exit 1;
            }
        }

        Get-ExternalTools

        $TestAssemblies = Get-ChildItem -Recurse "$NCryptTestsFolder\*.Tests.dll" |Where-Object { $_.Directory -notlike '*netcoreapp*' }
        Write-Host "Test assemblies: $TestAssemblies"
        $xunitx86Runner = Join-Path $PackageRestoreRoot 'xunit.runner.console/2.4.1/tools/net472/xunit.console.x86.exe'
        $xunitx64Runner = Join-Path $PackageRestoreRoot 'xunit.runner.console/2.4.1/tools/net472/xunit.console.exe'


        $xunitArgs = @()
        $xunitArgs += $TestAssemblies

        $xunitx86Args += "-html","$BinTestsFolder\$LogFileName.html","-xml","$BinTestsFolder\$LogFileName.x86.xml"
        $xunitx64Args += "-html","$BinTestsFolder\$LogFileName.html","-xml","$BinTestsFolder\$LogFileName.x64.xml"

        $xunitArgs += "-notrait",'"skiponcloud=true"'
        if (!$NoParallelTests) {
            $xunitArgs += "-parallel","all"
        }

        Write-Host "Testing x86..." -ForegroundColor Yellow
        Write-Host "$xunitx86Runner $xunitArgs $xunitx86Args" -ForegroundColor Yellow
        & $xunitx86Runner $xunitArgs $xunitx86Args| Out-Host
        if ($LASTEXITCODE -ne 0) {
            Write-Error "x86 test run returned exit code $LASTEXITCODE"
            $testsFailed = $true
        }

        Write-Host "Testing x64..." -ForegroundColor Yellow
        Write-Host "$xunitx64Runner $xunitArgs $xunitx64Args"-ForegroundColor Yellow
        & $xunitx64Runner $xunitArgs $xunitx64Args| Out-Host
        if ($LASTEXITCODE -ne 0) {
            Write-Error "x64 test run returned exit code $LASTEXITCODE"
            $testsFailed = $true
        }

        if ($testsFailed) {
            exit 4
        }
    } -ArgumentList $ProjectRoot,"testresults$i"
    $jobs += $j
}

Receive-Job -Job $jobs -Wait
