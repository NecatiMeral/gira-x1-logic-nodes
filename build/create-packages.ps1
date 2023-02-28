[CmdletBinding()]
param (
    [Parameter(Mandatory = $True)]
    [string]
    $SigniningCertFile,
    [Parameter(Mandatory = $True)]
    [string]
    $SigniningCertPass
)

$DistDir = Resolve-Path (Join-Path $PSScriptRoot '../dist')
$SigningScriptPath = Resolve-Path (Join-Path $PSScriptRoot 'sign-node.ps1')

Write-Host "Start creating logic node packages at ${DistDir}" -ForegroundColor Blue

$Packages = Get-ChildItem $DistDir -Filter '*.zip'

foreach ($Package in $Packages) {

    $PackagePath = $Package.FullName
    $SignNodeArgs = @(
        '-ZipFile'
        "${PackagePath}"
        '-Cert'
        "${SigniningCertFile}"
        '-Pass'
        "${SigniningCertPass}"
    )

    Write-Host "${SigningScriptPath} -ZipFile ${PackagePath} -Cert ${SigniningCertFile} -Pass ${SigniningCertPass}" -ForegroundColor Magenta
    . $SigningScriptPath -ZipFile ${PackagePath} -Cert ${SigniningCertFile} -Pass ${SigniningCertPass}
}
