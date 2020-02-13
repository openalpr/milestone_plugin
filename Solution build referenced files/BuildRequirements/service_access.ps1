param (
  [alias('computer', 'c')]
  [parameter(mandatory=$true)]
  [string] $computerName = $env:COMPUTERNAME,

  [alias('username', 'u')]
  [parameter(mandatory=$true)]
  [string] $serviceUsername = "$env:USERDOMAIN\$env:USERNAME",

  [alias('password', 'p')]
  [parameter(mandatory=$true)]
  [string] $servicePassword
)

Invoke-Command -ComputerName $computerName -Script {
  param(
    [string] $computerName,
    [string] $serviceUsername,
    [string] $servicePassword
  )
  Get-WmiObject -ComputerName $computerName -Namespace root\cimv2 -Class Win32_Service | Where-Object { $_.StartName -eq $serviceUsername } | ForEach-Object {
    Write-Host ("Setting credentials for service: {0} (username: {1}), on host: {2}." -f $_.Name, $serviceUsername, $computerName)
    $change = $_.Change($null, $null, $null, $null, $null, $null, $serviceUsername, $servicePassword).ReturnValue
    if ($change -eq 0) {
      Write-Host ("Service Change() request accepted.")
      if ($_.Started) {
        $serviceName = $_.Name
        Write-Host ("Restarting service: {0}, on host: {1}, to implement credential change." -f $serviceName, $computerName)
        $stop = ($_.StopService()).ReturnValue
        if ($stop -eq 0) {
          Write-Host -NoNewline ("StopService() request accepted. Awaiting 'stopped' status.")
          while ((Get-WmiObject -ComputerName $computerName -Namespace root\cimv2 -Class Win32_Service -Filter "Name='$serviceName'").Started) {
            Start-Sleep -s 2
            Write-Host -NoNewline "."
          }
          Write-Host "."
          $start = $_.StartService().ReturnValue
          if ($start -eq 0) {
            Write-Host ("StartService() request accepted.")
          } else {
            Write-Host ("Failed to start service. ReturnValue was '{0}'. See: http://msdn.microsoft.com/en-us/library/aa393660(v=vs.85).aspx" -f $start) -ForegroundColor "red"
          }
        } else {
          Write-Host ("Failed to stop service. ReturnValue was '{0}'. See: http://msdn.microsoft.com/en-us/library/aa393673(v=vs.85).aspx" -f $stop) -ForegroundColor "red"
        }
      }
    } else {
      Write-Host ("Failed to change service credentials. ReturnValue was '{0}'. See: http://msdn.microsoft.com/en-us/library/aa384901(v=vs.85).aspx" -f $change) -ForegroundColor "red"
    }
  }
} -Credential "$env:USERDOMAIN\$env:USERNAME" -ArgumentList $computerName, $serviceUsername, $servicePassword