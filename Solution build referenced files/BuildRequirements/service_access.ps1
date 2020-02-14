param (
  [alias('password', 'p')]
  [parameter(mandatory=$true)]
  [string] $servicePassword
)

if(((Get-LocalGroupMember -Group 'Administrators' | select Name) -match [System.Security.Principal.WindowsIdentity]::GetCurrent().Name.Split("\")[1]).Count -eq 0) {
   Add-LocalGroupMember -Group 'Administrators' -Member ([System.Security.Principal.WindowsIdentity]::GetCurrent().Name) –Verbose
}

$ServiceAccount = [System.Security.Principal.WindowsIdentity]::GetCurrent().Name
$ServicePass    = $servicePassword
$Computer       = [System.Net.Dns]::GetHostName()
$ServiceName    = "OpenALPRMilestone"

$ServiceObject  = Get-WmiObject -ComputerName "$Computer" -Query "SELECT * FROM Win32_Service WHERE Name = '$ServiceName'"

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


$ServiceObject.stopservice() | out-null
$ServiceObject.Change($null,$null,$null,$null,$null,$null,$ServiceAccount,$ServicePass,$null,$null,$null)
$ServiceObject.startservice()