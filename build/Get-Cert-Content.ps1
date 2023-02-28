[CmdletBinding()]
param (
    [Parameter(Mandatory = $True)]
    [string]
    $Cert,
    [Parameter(Mandatory = $True)]
    [string]
    $Pass
)

$flag = [System.Security.Cryptography.X509Certificates.X509KeyStorageFlags]::Exportable
$collection = New-Object System.Security.Cryptography.X509Certificates.X509Certificate2Collection
$collection.Import($Cert, $Pass, $flag)
$pkcs12ContentType = [System.Security.Cryptography.X509Certificates.X509ContentType]::Pkcs12
$clearBytes = $collection.Export($pkcs12ContentType)
$fileContentEncoded = [System.Convert]::ToBase64String($clearBytes)
$secret = ConvertTo-SecureString -String $fileContentEncoded -AsPlainText –Force

Write-Host $fileContentEncoded
