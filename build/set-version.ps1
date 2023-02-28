[CmdletBinding()]
param (
    [Parameter(Mandatory = $True)]
    [string]
    $Version
)

$RepositoryRoot = Resolve-Path (Join-Path $PSScriptRoot '../')
$BuildFile = Join-Path $RepositoryRoot 'dotnet\Directory.Build.props'
$ManifestFiles = Get-ChildItem $RepositoryRoot -Recurse `
    | Where-Object { $_.Name -eq 'Manifest.json' -and $_.FullName -notmatch 'bin|obj' }

Write-Host "Build.props: ${BuildFile}"

$BuildFileContent = Get-Content $BuildFile
$BuildFileContent = $BuildFileContent -Replace '<Version>([\w\d\.]*)<\/Version>', "<Version>${Version}</Version>"
Set-Content $BuildFile $BuildFileContent


foreach ($File in $ManifestFiles) {
    Write-Host "Manifest: ${File}"

    $ManifestContent = Get-Content $File
    $ManifestContent = $ManifestContent -Replace '"Version": "(.*)"', """Version"": ""${Version}"""

    Set-Content $File $ManifestContent
}
