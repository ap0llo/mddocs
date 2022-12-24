$ErrorActionPreference = "Stop"

if ($env:TF_BUILD) {
    Write-Host "##[group]Install .NET SDK"
}

# Install .NET 6 SDK (required for running tests on this platform)
# The MSBuild Integration tests require the SDK to be present, so just installing the runtime is not sufficient
./build/dotnet-install.ps1 -Version 6.0.404

# Install SDK and runtime as specified in global.json
./build/dotnet-install.ps1 -JsonFile "$PSScriptRoot/global.json"

Invoke-Expression "dotnet --info"

if ($env:TF_BUILD) {
    Write-Host "##[endgroup]"
}

dotnet run --project build/Build.csproj -- $args
exit $LASTEXITCODE