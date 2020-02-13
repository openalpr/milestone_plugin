$ServiceAccount = ".\Administrator"
$ServicePass    = "Password"
$Computer       = "."
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

Write-Output 	$DisplayName,
				$PathName,
				$ServiceType,
				$ErrorControl,
				$StartMode,
				$DesktopInteract,
				$StartName,
				$StartPassword,
				$LoadOrderGroup,
				$LoadOrderGroupDependencies,
				$ServiceDependencies

$ServiceObject.stopservice() | out-null
$ServiceObject.Change($DisplayName,$PathName,$ServiceType,$ErrorControl,$StartMode,$DesktopInteract,$ServiceAccount,$ServicePass,$LoadOrderGroup,$LoadOrderGroupDependencies,$ServiceDependencies)
$ServiceObject.startservice()