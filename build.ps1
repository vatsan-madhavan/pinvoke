<#
.Synopsis
    Acquires dependencies, builds, and tests this project.
.Description
    If no actions are specified, the default is to run all actions.
.Parameter Restore
    Restore NuGet packages.
.Parameter Build
    Build the entire project. Requires that -Restore is or has been executed.
.Parameter Rebuild
    Build the entire project with the Rebuild target. This may be slower than -Build but ensures you'll see any build warnings from prior builds. Requires that -Restore is or has been executed.
.Parameter Test
    Run all built tests.
.Parameter Configuration
    The configuration to build. Either "debug" or "release". The default is debug, or the Configuration environment variable if set.
.Parameter WarnAsError
    Converts all build warnings to errors. Useful in preparation to sending a pull request.
.Parameter GeneratePInvokesTxt
    Produces the LIBNAME.pinvokes.txt files along with the assemblies during the build.
.Parameter NoParallelTests
    Do not execute tests in parallel.
.Parameter BinaryLog
	Generate MSBuild Binary Log (in addition to text logs)
#>
[CmdletBinding(SupportsShouldProcess=$true,ConfirmImpact='Medium')]
Param(
    [switch]$Restore,
    [switch]$Build,
    [switch]$Rebuild,
    [switch]$Test,
    [Parameter()][ValidateSet('debug', 'release')]
    [string]$Configuration = $env:BUILDCONFIGURATION,
    [switch]$WarnAsError = $true,
    [switch]$GeneratePInvokesTxt,
    [switch]$NoParallelTests,
	[Alias('bl')]
	[switch]$BinaryLog
)

if (!$Configuration) { $Configuration = 'debug' }

$NothingToDo = !($Restore -or $Build -or $Test)
if ($NothingToDo) {
    Write-Output "No actions specified. Applying default actions."
    $Restore = $true
    $Build = $true
    $Test = $true
}

# External dependencies
$sourceNugetExe = "https://dist.nuget.org/win-x86-commandline/v3.3.0/nuget.exe"

# Path variables
$ProjectRoot = Split-Path -parent $PSCommandPath
$SolutionFolder = Join-Path $ProjectRoot src
$SolutionFile = Join-Path $SolutionFolder "PInvoke.sln"
$ToolsFolder = Join-Path $ProjectRoot tools
$BinFolder = Join-Path $ProjectRoot "bin"
$BinConfigFolder = Join-Path $BinFolder $Configuration
$BinTestsFolder = Join-Path $BinConfigFolder "tests"
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

if ($Restore -and $PSCmdlet.ShouldProcess($SolutionFile, "Restore packages")) {
    Write-Output "Restoring NuGet packages..."
    & "$PSScriptRoot\init.ps1"
}

if (($Build -or $Rebuild) -and $PSCmdlet.ShouldProcess($SolutionFile, "Build")) {
    $msbuildTarget = "build"
    if ($Rebuild) { $msbuildTarget = "rebuild" }
    $buildArgs = @()
    $buildArgs += $SolutionFile,'/nologo','/nr:false','/m','/v:minimal',"/t:$msbuildTarget,pack"
    $buildArgs += "/p:Configuration=$Configuration"
    $buildArgs += "/clp:ForceNoAlign;Summary"
    $buildArgs += '/fl','/flp:verbosity=normal;logfile=msbuild.log','/flp1:warningsonly;logfile=msbuild.wrn;NoSummary;verbosity=minimal','/flp2:errorsonly;logfile=msbuild.err;NoSummary;verbosity=minimal'
    if ($GeneratePInvokesTxt) {
        $buildArgs += '/p:GeneratePInvokesTxt=true'
    }
	if ($BinaryLog) {
		$buildArgs += '/bl'
	}

    Write-Output "Building..."
    & $MSBuildCommand.Path $buildArgs
    $fail = $false

    $warnings = Get-Content msbuild.wrn
    $errors = Get-Content msbuild.err
    $WarningsPrompt = "$(($warnings | Measure-Object -Line).Lines) warnings during build"
    $ErrorsPrompt = "$(($errors | Measure-Object -Line).Lines) errors during build"
    if ($errors.length -gt 0) {
        Write-Error $ErrorsPrompt
        $fail = $true
    } else {
        Write-Output $ErrorsPrompt
    }

    if ($WarnAsError -and $warnings.length -gt 0) {
        Write-Error $WarningsPrompt
        $fail = $true
    } elseif ($warnings.length -gt 0) {
        Write-Warning $WarningsPrompt
    } else {
        Write-Output $WarningsPrompt
    }

    if ($fail) {
        exit 3;
    }
}

if ($Test -and $PSCmdlet.ShouldProcess('Test assemblies')) {
    $TestAssemblies = Get-ChildItem -Recurse "$BinTestsFolder\*.Tests.dll" |? { $_.Directory -notlike '*netcoreapp*' }
    $xunitArgs = @()
    $xunitArgs += $TestAssemblies
    $xunitArgs += "-html","$BinTestsFolder\testresults.html","-xml","$BinTestsFolder\testresults.xml"
    $xunitArgs += "-notrait",'"skiponcloud=true"'
    if (!$NoParallelTests) {
        $xunitArgs += "-parallel","all"
    }

    Write-Host "Testing x86..." -ForegroundColor Yellow
    $xunitRunner = Join-Path $PackageRestoreRoot 'xunit.runner.console/2.4.1/tools/net472/xunit.console.x86.exe'
    & $xunitRunner $xunitArgs
    if ($LASTEXITCODE -ne 0) {
        Write-Error "x86 test run returned exit code $LASTEXITCODE"
        $testsFailed = $true
    }

    Write-Host "Testing x64..." -ForegroundColor Yellow
    $xunitRunner = Join-Path $PackageRestoreRoot 'xunit.runner.console/2.4.1/tools/net472/xunit.console.exe'
    & $xunitRunner $xunitArgs
    if ($LASTEXITCODE -ne 0) {
        Write-Error "x64 test run returned exit code $LASTEXITCODE"
        $testsFailed = $true
    }

    if ($testsFailed) {
        exit 4
    }
}
