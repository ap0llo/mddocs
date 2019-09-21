$repoRoot = Resolve-Path (Join-Path $PSScriptRoot "..")

# check if dotnet CLI is installed
$dotnetCommand = Get-Command -Name "dotnet" -CommandType Application
if($null -eq $dotnetCommand) {
    throw "dotnet.exe not found"
}

$demoProjects = @(
    #(Join-Path $repoRoot "src\MdDocs.ApiReference.DemoProject\Grynwald.MdDocs.ApiReference.DemoProject.csproj"),
    (Join-Path $repoRoot "src\MdDocs.CommandLineHelp.DemoProject\Grynwald.MdDocs.CommandLineHelp.DemoProject.csproj")
)

foreach($demoProject in $demoProjects) {

    Write-Host "Updating docs for demo project '$demoProject'"
    $command = "  dotnet msbuild /t:UpdateDocs `"$demoProject`""
    Invoke-Expression "$command"
    if($LASTEXITCODE -ne 0) {
        throw "Command '$command' failed with exit code $LASTEXITCODE "
    }
}
