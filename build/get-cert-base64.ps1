[CmdletBinding()]
param (
    [Parameter(Mandatory = $True)]
    [string]
    $Cert
)
$pfx_cert = [System.IO.File]::ReadAllBytes($Cert)
[System.Convert]::ToBase64String($pfx_cert) | Out-File 'SigningCertificate_Encoded.txt'
