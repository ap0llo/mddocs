#
# This script creates a new release branch for the current version, increments 
# the version on master and pushes both master and the new release branch.
#

# Imports
. (Join-Path $PSScriptRoot "common.ps1")

# Variables
$expectedCurrentBranch = "master"

# Functions
function checkoutAndPush($branchName) {

    log "Switching to branch $branchName"
    exec "git checkout `"$branchName`"" | Write-Host

    log "Pushing branch $branchName"
    exec "git push --set-upstream origin `"$branchName`"" | Write-Host
}

function copySchema {
    Push-Location (Get-RepositoryRoot)
    try {

        # determine the major/minor version being released
        $semanticVersion = New-Object -TypeName "System.Management.Automation.SemanticVersion" -ArgumentList ((Get-Version).SemVer2)
        $majorMinor = "$($semanticVersion.Major).$($semanticVersion.Minor)"

        # Get current configuration schema
        $latestSchemaPath = "./schemas/configuration/schema.json"

        # Copy current schema to version-specific schema path
        $versionSpecificSchemaDirectory = "./schemas/configuration/v$majorMinor/"
        New-Item -ItemType Directory -Path $versionSpecificSchemaDirectory  | Out-Null
        Copy-Item -Path $latestSchemaPath  -Destination $versionSpecificSchemaDirectory
                
        # Commit changes
        exec "git add `"$versionSpecificSchemaDirectory`""
        exec "git commit -m `"Add v$majorMinor configuration schema`" "
    }
    finally {
        Pop-Location
    }    
}


# Main script
Push-Location (Get-RepositoryRoot)
try {

    copySchema

    # Ensure we're on the master branch
    $currentBranch = exec "git rev-parse --abbrev-ref HEAD"
    if ($currentBranch -ne $expectedCurrentBranch) {
        throw "New releases must be created from master"
    }

    log "Restoring tools"
    exec "dotnet tool restore"

    log "Creating release branch"
    $nbgvOutput = exec "dotnet tool run nbgv -- prepare-release"
    Write-Host $nbgvOutput

    # Find the name of the release branch
    $outputLines = $nbgvOutput.Split([System.Environment]::NewLine)
    $releaseBranchName = $null
    foreach ($line in $outputLines) {
        # The line logging the release branch name looks something like this:
        # 'release/v0.2 branch now tracks v0.2 stabilization and release.'
        if ($line.Contains("stabilization and release") -and $line.Contains("branch")) {
            
            $releaseBranchName = $line.Substring(0, $line.IndexOf("branch")).Trim()
        }
    }

    if ($releaseBranchName) {
        log "Release branch name is '$releaseBranchName'"
    }
    else {
        throw "Failed to determine name of release branch"
    }

    checkoutAndPush $releaseBranchName
    checkoutAndPush $currentBranch
}
finally {
    Pop-Location
}
