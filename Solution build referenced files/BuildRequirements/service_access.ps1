param (
  [parameter(mandatory=$true)]
  [String] $UserName,
  [alias('password', 'p')]
  [parameter(mandatory=$true)]
  [String] $ServicePassword
  ##[SecureString] $ServicePassword
)

$ServiceAccount  = [System.Security.Principal.WindowsIdentity]::GetCurrent().Name
$Computer        = [System.Net.Dns]::GetHostName()
$ServiceName     = "OpenALPRMilestone"
$DisplayLanguage = GET-WinSystemLocale | select Name

$ServiceObject   = Get-WmiObject -ComputerName "$Computer" -Query "SELECT * FROM Win32_Service WHERE Name = '$ServiceName'"
##$ServiceObject = Get-Service $ServiceName

$DisplayName = $($ServiceObject | select DisplayName)
$PathName = $($ServiceObject | select PathName)
$ServiceType = $($ServiceObject | select ServiceType)
$ErrorControl = $($ServiceObject | select ErrorControl)
$StartMode = $($ServiceObject | select StartMode)
$DesktopInteract = $($ServiceObject | select DesktopInteract)
$StartName = $($ServiceObject | select StartName)
$StartPassword = $($ServiceObject | select StartPassword)
$LoadOrderGroup = $($ServiceObject | select LoadOrderGroup)
$LoadOrderGroupDependencies = $($ServiceObject | select LoadOrderGroupDependencies)
$ServiceDependencies = $($ServiceObject | select ServiceDependencies)
$LoginName = ($Computer + "\" + $UserName)

if ($UserName -eq "Network Service")
{
    $LoginName = "NT AUTHORITY\NETWORK SERVICE"
}

switch($ServiceType.ServiceType.ToString()){
    "Kernel Driver" {$ServiceType = 0x1}
    "File System Driver" {$ServiceType = 0x2}
    "Adapter" {$ServiceType = 0x4}
    "Recognizer Driver" {$ServiceType = 0x8}
    "Own Process" {$ServiceType = 0x10}
    "Share Process" {$ServiceType = 0x20}
    "Interactive Process" {$ServiceType = 0x100}
}

switch($ErrorControl.ErrorControl.ToString()){
    "Ignore"   {$ErrorControl = 0x0}
    "Normal"   {$ErrorControl = 0x1}
    "Severe"    {$ErrorControl = 0x2}
    "Critical"  {$ErrorControl = 0x3}
}

$Xprotect = Get-Process Client -ErrorAction SilentlyContinue
if ($Xprotect) {
  $Xprotect.CloseMainWindow()

  if (!$Xprotect.HasExited) {
    $Xprotect | Stop-Process -Force
  }
}
Remove-Variable Xprotect

$ServiceObject.stopservice() | out-null
$ServiceObject.Change($null,$null,$null,$null,$null,$null,$LoginName,$ServicePassword,$null,$null,$null)
$ServiceObject.startservice()