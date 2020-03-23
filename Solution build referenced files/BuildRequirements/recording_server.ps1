$Computer               = [System.Net.Dns]::GetHostName()
$RecordingServiceName   = "Milestone XProtect Recording Server"
$RecordingServiceObject = Get-Service $RecordingServiceName
$OneTimeStopped = $false
$OneTimeStarted = $false

Remove-Item -Path "$($env:ALLUSERSPROFILE)\Milestone\XProtect Recording Server\DriverCache\NativeDrivers\_cache" -Force

while ($OneTimeStarted -eq $false)
{
    if ((($RecordingServiceObject.Status -ne 'Stopped') -or ($RecordingServiceObject.Status -ne 'StopPending')) -and ($OneTimeStopped -eq $false))
    {
        Stop-Service $RecordingServiceName
        $OneTimeStopped = $true
    }

    if ((($RecordingServiceObject.Status -ne 'Running') -or ($RecordingServiceObject.Status -ne 'StopPending')) -and ($OneTimeStarted -eq $false))
    {
        Start-Service $RecordingServiceName
        $OneTimeStarted = $true
    }
}
$RecordingServiceObject.Status