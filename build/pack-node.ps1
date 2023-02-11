[CmdletBinding()]
param (
    [Parameter(Mandatory = $True)]
    [string]
    $Project
)

Write-Host "Packing logic-node project at ``${Project}``" -ForegroundColor Blue
$RepositoryRoot = Resolve-Path (Join-Path $PSScriptRoot '../')
$SdkDir = Join-path $PSScriptRoot 'tools/gira'
$DistDir = Resolve-Path (Join-Path $PSScriptRoot '../dist')

Write-Host "Repository: ``${RepositoryRoot}``"
Write-Host "SDK-Tools:  ``${SdkDir}``"
Write-Host "Output:     ``${DistDir}``"

$NodeToolPath = Join-Path -Path $SdkDir -ChildPath 'LogicNodeTool.exe'

$CreateNodeArgs = @(
    'create'
    "${Project}"
    "${DistDir}"
)

Write-Host "${NodeToolPath} ${CreateNodeArgs}" -ForegroundColor Magenta
. $NodeToolPath $CreateNodeArgs
