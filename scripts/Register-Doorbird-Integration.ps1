[CmdletBinding()]
param (
    [Parameter(Mandatory=$true)][string]$HostOrIp,
    [Parameter(Mandatory=$true)][string]$ClientId,
    [Parameter(Mandatory=$true)][string]$Username,
    [Parameter(Mandatory=$true)][string]$Password,
    [Parameter()][string]$DoorbirdTriggerName = $("Klingel auslösen")
)

$ErrorActionPreference = 'Stop'

$ApiAvailabilityUrl = "https://${HostOrIp}/api/v2/"
$RegisterClientUrl = "https://${HostOrIp}/api/v2/clients"

# Testing connectivity
Write-Host "Testing X1 connectivity..." -ForegroundColor Blue

Write-Host "Invoke-RestMethod -Uri ${ApiAvailabilityUrl} -SkipCertificateCheck" -ForegroundColor Magenta
$HealhCheckResponse = Invoke-RestMethod -Uri ${ApiAvailabilityUrl} -SkipCertificateCheck

Write-Host "Connection to '$($HealhCheckResponse.deviceName)' (Version $($HealhCheckResponse.deviceVersion)) succeeded" -ForegroundColor Blue

$credPair = "$($Username):$($Password)"
$encodedCredentials = [System.Convert]::ToBase64String([System.Text.Encoding]::ASCII.GetBytes($credPair))
$Headers = @{
    Authorization = "Basic $encodedCredentials"
}

# Register client (if not exists) and get token
$RegisterClientBody = @{
    client = $ClientId
} | ConvertTo-Json -Depth 2

Write-Host "Registering Client '${ClientId}'..." -ForegroundColor Blue
Write-Host "Invoke-RestMethod -Method POST -Uri ${RegisterClientUrl} -SkipCertificateCheck -Headers $Headers -Body $RegisterClientBody" -ForegroundColor Magenta
$TokenResponse = Invoke-RestMethod -Method POST -Uri ${RegisterClientUrl} -SkipCertificateCheck -Headers $Headers -Body $RegisterClientBody
$ClientToken = $TokenResponse.token

Write-Host "Received Token '${ClientToken}'" -ForegroundColor Blue

# Get UI config to validate trigger element ui
$GetUiConfigUrl = "https://${HostOrIp}/api/v2/uiconfig?token=${ClientToken}"

Write-Host "Searching for Doorbird Trigger in UI configuration..." -ForegroundColor Blue
Write-Host "Invoke-RestMethod -Uri ${GetUiConfigUrl} -SkipCertificateCheck -Headers $Headers" -ForegroundColor Magenta
$UiConfig = Invoke-RestMethod -Uri ${GetUiConfigUrl} -SkipCertificateCheck -Headers $Headers

Write-Host "Got $($UiConfig.functions.Count) UI elements. There's still plenty of room. ;-)" -ForegroundColor Blue
$DoorbirdTriggerElement = $UiConfig.functions | Where-Object { $_.displayName -eq $DoorbirdTriggerName }

if($null -eq $DoorbirdTriggerElement)
{
    Write-Host "Could not find doorbird trigger function. Searched for display name '${DoorbirdTriggerName}'." -ForegroundColor Red
    Write-Host "Please create a trigger [in german: 'Taster (Drücken/Loslassen)'] in the GPA app, apply the configuration and try again." -ForegroundColor Red
    exit 1
}

Write-Host "Got doordbird trigger function with uid '$($DoorbirdTriggerElement.uid)'" -ForegroundColor Blue

# TODO: Support more channelTypes
if($DoorbirdTriggerElement.channelType -ne 'de.gira.schema.channels.Trigger')
{
    Write-Host "Unsupported Doorbird trigger function type." -ForegroundColor Red
    Write-Host "Currently only 'Trigger' elements [in german: 'Taster (Drücken/Loslassen)'] are supported." -ForegroundColor Red
    exit 1
}

$DataPoint = $DoorbirdTriggerElement.dataPoints | Where-Object { $_.name -eq 'Trigger' }

if($null -eq $DataPoint)
{
    Write-Host "Failed to locate datapoint, this shouldn't occur! Please report this error so we can fix it." -ForegroundColor Red
    exit 1
}

$DataPointUid = $DataPoint.uid
$TriggerUrl = "https://${HostOrIp}/api/v2/values/${DataPointUid}?token=${ClientToken}"
Write-Host "Here's the URL to use with your doorbird:" -ForegroundColor Green
Write-Host "${TriggerUrl}" -ForegroundColor Magenta
Write-Host "Please use this URL with the following payload:" -ForegroundColor Green
Write-Host "{ ""value"": ""1"" }" -ForegroundColor Magenta
