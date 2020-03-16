param (
  [parameter(mandatory=$true)]
  [String] $UserName,
  [alias('password', 'p')]
  [parameter(mandatory=$true)]
  [SecureString] $ServicePassword
)

try {
    if(((Get-LocalGroupMember -Group 'Administrators' -ErrorAction Ignore | select Name) -match [System.Security.Principal.WindowsIdentity]::GetCurrent()).Count -eq 0) {
       Add-LocalGroupMember -Group 'Administrators' -Member ([System.Security.Principal.WindowsIdentity]::GetCurrent().Name) –Verbose -ErrorAction SilentlyContinue
    }
}
catch {#[Microsoft.PowerShell.Commands.MemberExistsException] { 
	#Write-Output "Error to add current user to Administrators role"
}

try {
    if(((Get-LocalGroupMember -Group 'Администраторы' -ErrorAction Ignore | select Name) -match [System.Security.Principal.WindowsIdentity]::GetCurrent()).Count -eq 0) {
       Add-LocalGroupMember -Group 'Администраторы' -Member ([System.Security.Principal.WindowsIdentity]::GetCurrent().Name) –Verbose -ErrorAction SilentlyContinue
    }
}
catch {#[Microsoft.PowerShell.Commands.MemberExistsException] { 
	#Write-Output "Error to add current user to Administrators role"
}

try {
    if(((Get-LocalGroupMember -Group 'Administrateurs' -ErrorAction Ignore | select Name) -match [System.Security.Principal.WindowsIdentity]::GetCurrent()).Count -eq 0) {
       Add-LocalGroupMember -Group 'Administrateurs' -Member ([System.Security.Principal.WindowsIdentity]::GetCurrent().Name) –Verbose -ErrorAction SilentlyContinue
    }
}
catch {#[Microsoft.PowerShell.Commands.MemberExistsException] { 
	#Write-Output "Error to add current user to Administrators role"
}

try {
    if(((Get-LocalGroupMember -Group 'Administratrices' -ErrorAction Ignore | select Name) -match [System.Security.Principal.WindowsIdentity]::GetCurrent()).Count -eq 0) {
       Add-LocalGroupMember -Group 'Administratrices' -Member ([System.Security.Principal.WindowsIdentity]::GetCurrent().Name) –Verbose -ErrorAction SilentlyContinue
    }
}
catch {#[Microsoft.PowerShell.Commands.MemberExistsException] { 
	#Write-Output "Error to add current user to Administrators role"
}

try {
    if(((Get-LocalGroupMember -Group 'Administratorer' -ErrorAction Ignore | select Name) -match [System.Security.Principal.WindowsIdentity]::GetCurrent()).Count -eq 0) {
       Add-LocalGroupMember -Group 'Administratorer' -Member ([System.Security.Principal.WindowsIdentity]::GetCurrent().Name) –Verbose -ErrorAction SilentlyContinue
    }
}
catch {#[Microsoft.PowerShell.Commands.MemberExistsException] { 
	#Write-Output "Error to add current user to Administrators role"
}

try {
    if(((Get-LocalGroupMember -Group 'Administratorinnen' -ErrorAction Ignore | select Name) -match [System.Security.Principal.WindowsIdentity]::GetCurrent()).Count -eq 0) {
       Add-LocalGroupMember -Group 'Administratorinnen' -Member ([System.Security.Principal.WindowsIdentity]::GetCurrent().Name) –Verbose -ErrorAction SilentlyContinue
    }
}
catch {#[Microsoft.PowerShell.Commands.GroupExistsException] { 
	#Write-Output "Error to add current user to Administrators role"
}

try {
    if(((Get-LocalGroupMember -Group 'Administratoren' -ErrorAction Ignore | select Name) -match [System.Security.Principal.WindowsIdentity]::GetCurrent()).Count -eq 0) {
       Add-LocalGroupMember -Group 'Administratoren' -Member ([System.Security.Principal.WindowsIdentity]::GetCurrent().Name) –Verbose -ErrorAction SilentlyContinue
    }
}
catch {#[Microsoft.PowerShell.Commands.MemberExistsException] { 
	#Write-Output "Error to add current user to Administrators role"
}

try {
    if(((Get-LocalGroupMember -Group 'Administradoras' -ErrorAction Ignore | select Name) -match [System.Security.Principal.WindowsIdentity]::GetCurrent()).Count -eq 0) {
       Add-LocalGroupMember -Group 'Administradoras' -Member ([System.Security.Principal.WindowsIdentity]::GetCurrent().Name) –Verbose -ErrorAction SilentlyContinue
    }
}
catch {#[Microsoft.PowerShell.Commands.MemberExistsException] { 
	#Write-Output "Error to add current user to Administrators role"
}

try {
    if(((Get-LocalGroupMember -Group 'Administradores' -ErrorAction Ignore | select Name) -match [System.Security.Principal.WindowsIdentity]::GetCurrent()).Count -eq 0) {
       Add-LocalGroupMember -Group 'Administradores' -Member ([System.Security.Principal.WindowsIdentity]::GetCurrent().Name) –Verbose -ErrorAction SilentlyContinue
    }
}
catch {#[Microsoft.PowerShell.Commands.MemberExistsException] { 
	#Write-Output "Error to add current user to Administrators role"
}

try {
    if(((Get-LocalGroupMember -Group 'Administrators' -ErrorAction Ignore | select Name) -match [System.Security.Principal.WindowsIdentity]::GetCurrent()).Count -eq 0) {
       Add-LocalGroupMember -Group 'Administrators' -Member ([System.Security.Principal.WindowsIdentity]::GetCurrent().Name) –Verbose -ErrorAction SilentlyContinue
    }
}
catch {#{#[Microsoft.PowerShell.Commands.MemberExistsException] {
	#Write-Output "Error to add current user to Administrators role" 
}

$ServiceAccount  = [System.Security.Principal.WindowsIdentity]::GetCurrent().Name
$ServicePass     = $ServicePassword
$Computer        = [System.Net.Dns]::GetHostName()
$ServiceName     = "OpenALPRMilestone"
$DisplayLanguage = GET-WinSystemLocale | select Name

$ServiceObject   = Get-WmiObject -ComputerName "$Computer" -Query "SELECT * FROM Win32_Service WHERE Name = '$ServiceName'"

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

$BSTR = [System.Runtime.InteropServices.Marshal]::SecureStringToBSTR($ServicePass)
$Password = [System.Runtime.InteropServices.Marshal]::PtrToStringAuto($BSTR)

$ServiceObject.stopservice() | out-null
$ServiceObject.Change($null,$null,$null,$null,$null,$null,$Computer + "\" + $UserName,$Password,$null,$null,$null)
$ServiceObject.startservice()