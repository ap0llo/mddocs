#
# This scripts runs "dotnet test" for the project, collects code coverage and gernated a coverage report
#

# Imports
. (Join-Path $PSScriptRoot "common.ps1")

# Variables
$testResultsDirectory = ".\Build\TestResults"
$coverageHistoryDirectory = ".\Build\CoverageHistory"

# Main script
Push-Location (Get-RepositoryRoot)
try {

    log "Restoring tools"
    exec "dotnet tool restore"

    if (Test-Path $testResultsDirectory) {
        log "Cleaning up test results directory"
        Remove-Item -Path $testResultsDirectory  -Recurse -Force
    }

    log "Running dotnet test with coverage"
    exec "dotnet test ./src/MdDocs.sln --collect:`"XPlat Code Coverage`" "

    log "Generating code coverage report"
    exec "dotnet tool run reportgenerator -- `"-reports:$testResultsDirectory\*\coverage.cobertura.xml`" `"-targetdir:$testResultsDirectory\Coverage`" `"-reporttypes:html`" `"-historyDir:$coverageHistoryDirectory`" "

    log "Code Coverage report generated to $((Get-location).Path)\$testResultsDirectory\Coverage\index.html"
}
finally {
    Pop-Location
}
