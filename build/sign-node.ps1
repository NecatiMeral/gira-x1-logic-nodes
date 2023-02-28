[CmdletBinding()]
param (
    [Parameter(Mandatory = $True)]
    [string]
    $ZipFile,
    [Parameter(Mandatory = $True)]
    [string]
    $Cert,
    [Parameter(Mandatory = $True)]
    [string]
    $Pass
)

$Location = Get-Location
Write-Host "Signing logic-node project at ``${ZipFile}``" -ForegroundColor Blue
$RepositoryRoot = Resolve-Path (Join-Path $PSScriptRoot '../')
$ZipFile = Resolve-Path $ZipFile
$Cert = Resolve-Path $Cert
$SdkDir = Join-path $PSScriptRoot 'tools/gira'
$DistDir = Resolve-Path (Join-Path $PSScriptRoot '../dist')

Write-Host "Repository: ``${RepositoryRoot}``"
Write-Host "SDK-Tools:  ``${SdkDir}``"
Write-Host "Output:     ``${DistDir}``"

$SignToolPath = Join-Path -Path $SdkDir -ChildPath 'SignLogicNodes.exe'

Set-Location $SdkDir

try {
    $SignNodeArgs = @(
        "${Cert}"
        "${Pass}"
        "${ZipFile}"
    )

    Write-Host "${SignToolPath} ${SignNodeArgs}" -ForegroundColor Magenta
    . $SignToolPath $SignNodeArgs
}
finally {
    Set-Location $Location
}
