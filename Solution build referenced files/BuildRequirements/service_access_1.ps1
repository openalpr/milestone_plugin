Function Set-ServiceAcctCreds([string]$strCompName,[string]$strServiceName,[string]$newAcct,[string]$newPass){
  $filter = 'Name=' + "'" + $strServiceName + "'" + ''
  $service = Get-WMIObject -ComputerName $strCompName -namespace "root\cimv2" -class Win32_Service -Filter $filter
  $service.Change($null,$null,$null,$null,$null,$null,$newAcct,$newPass)
  $service.StopService()
  while ($service.Started){
    sleep 2
    $service = Get-WMIObject -ComputerName $strCompName -namespace "root\cimv2" -class Win32_Service -Filter $filter
  }
  $service.StartService()
}

$ServiceAccount = "Username"
$ServicePass    = "Password"
$Computer       = "."
$ServiceName    = "OpenALPRMilestone"

$ServiceObject  = Get-WMIObject -Class Win32_Service -namespace "root\cimv2" -ComputerName $Computer -filter "Name='$ServiceName'"
$ServiceObject.stopservice() | out-null
$ServiceObject.Change($null,$null,$null,$null,$null,$ServiceAccount,$ServicePass,$null,$null,$null) | out-null
$ServiceObject.startservice()