
;--------------------------------
;Include Modern UI

	!include "MUI2.nsh"
	!include "DotNetChecker.nsh"
;--------------------------------
;General

	; The name of the installer
	Name "OpenALPR Milestone"

	; The file to write
	OutFile "OpenALPRMilestoneSetup.exe"

	; The default installation directory
	InstallDir $PROGRAMFILES64\VideoOS\MIPPlugins\OpenALPR

	; Request application privileges for Windows Vista
	RequestExecutionLevel admin

;--------------------------------
;Interface Configuration

  !define MUI_HEADERIMAGE
  !define MUI_HEADERIMAGE_BITMAP "C:\Openalpr\OpenALPRMilestone\OpenALPRPlugin\Resources\logo_bluegray.png" ; optional
  !define MUI_ABORTWARNING
  
;--------------------------------
; Pages
	!insertmacro MUI_PAGE_WELCOME
	!insertmacro MUI_PAGE_DIRECTORY
	!insertmacro MUI_PAGE_INSTFILES
  
	!insertmacro MUI_UNPAGE_CONFIRM
	!insertmacro MUI_UNPAGE_INSTFILES
	
;--------------------------------
;Languages
 
  !insertmacro MUI_LANGUAGE "English"

;--------------------------------
;Version Information

	VIProductVersion "1.0.0.0"
	VIAddVersionKey /LANG=${LANG_ENGLISH} "ProductName" "OpenALPRMilestone"
	VIAddVersionKey /LANG=${LANG_ENGLISH} "Comments" "A plug-in for Milestone Smart Client"
	VIAddVersionKey /LANG=${LANG_ENGLISH} "CompanyName" "OpenALPR Technology, Inc."
	VIAddVersionKey /LANG=${LANG_ENGLISH} "LegalTrademarks" "OpenALPRMilestone Application is a trademark of OpenALPR Technology"
	VIAddVersionKey /LANG=${LANG_ENGLISH} "LegalCopyright" "Copyright OpenALPR Technology"
	VIAddVersionKey /LANG=${LANG_ENGLISH} "FileDescription" "Intergrating OpenALPR with Milestone Smart Client"
	VIAddVersionKey /LANG=${LANG_ENGLISH} "FileVersion" "1.0.0.0"

;--------------------------------
; The stuff to install
Section "Files (required)"

  SectionIn RO
  
  ; https://github.com/ReVolly/NsisDotNetChecker
  ;!insertmacro CheckNetFramework 452
  !insertmacro CheckNetFramework 46
  ;!insertmacro CheckNetFramework 461
  ;!insertmacro CheckNetFramework 462
  ;!insertmacro CheckNetFramework 47
  ;!insertmacro CheckNetFramework 471
  
  ; Set output path to the service sub folder.
  SetOutPath "$INSTDIR\Service"
  
  File "..\build\OpenALPRQueueMilestone.exe"
  File "..\build\OpenALPRQueueMilestone.exe.config"
  
  File "..\Solution build referenced files\log4net\1.2.15\log4net.dll"
  File "..\Solution build referenced files\log4net\1.2.15\log4net.xml"
  
  File "..\Solution build referenced files\Newtonsoft\Newtonsoft.Json.dll"
  File "..\Solution build referenced files\Newtonsoft\Newtonsoft.Json.xml"
  
  File "..\Solution build referenced files\Beanstalk\Turbocharged.Beanstalk.dll"
  File "..\Solution build referenced files\Beanstalk\Turbocharged.Beanstalk.dll.config"
  File "..\Solution build referenced files\Beanstalk\Turbocharged.Beanstalk.xml"
  
  File "..\Solution build referenced files\Milestone\MIPSDK_2017R3\VideoOS.Platform.SDK.dll"
  File "..\Solution build referenced files\Milestone\MIPSDK_2017R3\VideoOS.Platform.dll"
  File "..\Solution build referenced files\Milestone\MIPSDK_2017R3\VideoOS.Platform.Primitives.dll"
  File "..\Solution build referenced files\Milestone\MIPSDK_2017R3\VideoOS.DatabaseUtility.Common.dll"
  File "..\Solution build referenced files\Milestone\MIPSDK_2017R3\VideoOS.DatabaseUtility.MediaStorage.dll"
  
  ; Set output path to the installation directory (Plug-in).
  SetOutPath $INSTDIR
  
  ; Put file there
  File "..\build\OpenALPRPlugin.dll"
  File "..\build\OpenALPRPlugin.dll.config"
  File "..\OpenALPRPlugin\plugin.def"
  File "..\Solution build referenced files\log4net\1.2.15\log4net.dll"
  File "..\Solution build referenced files\log4net\1.2.15\log4net.xml"
  
  ; Install a service - ServiceType own process - StartType automatic - NoDependencies - Logon as System Account
  SimpleSC::InstallService "OpenALPRMilestone" "OpenALPR Milestone" "16" "2" "$PROGRAMFILES64\VideoOS\MIPPlugins\OpenALPR\Service\OpenALPRQueueMilestone.exe" "" "" ""
  Pop $0 ; returns an errorcode (<>0) otherwise success (0)
  
  ; Set the description of a service
  SimpleSC::SetServiceDescription "OpenALPRMilestone" "Consume queue and save palte info as a bookmark in milestone systems"
  Pop $0 ; returns an errorcode (<>0) otherwise success (0)
  
  ; Start a service
  SimpleSC::StartService "OpenALPRMilestone" "" 30
  Pop $0 ; returns an errorcode (<>0) otherwise success (0)
  
  ; Write the installation path into the registry
  WriteRegStr HKLM SOFTWARE\OpenALPRMilestone "Install_Dir" "$INSTDIR"
  
  ; Write the uninstall keys for Windows 
  WriteRegStr HKLM "Software\Microsoft\Windows\CurrentVersion\Uninstall\OpenALPRMilestone" "DisplayName" "OpenALPR Milestone"
  WriteRegStr HKLM "Software\Microsoft\Windows\CurrentVersion\Uninstall\OpenALPRMilestone" "DisplayVersion" "1.0.0"
  WriteRegStr HKLM "Software\Microsoft\Windows\CurrentVersion\Uninstall\OpenALPRMilestone" "Publisher" "OpenALPR Technology, Inc."
  WriteRegStr HKLM "Software\Microsoft\Windows\CurrentVersion\Uninstall\OpenALPRMilestone" "UninstallString" '"$INSTDIR\uninstall.exe" /S'
  WriteRegDWORD HKLM "Software\Microsoft\Windows\CurrentVersion\Uninstall\OpenALPRMilestone" "NoModify" 1
  WriteRegDWORD HKLM "Software\Microsoft\Windows\CurrentVersion\Uninstall\OpenALPRMilestone" "NoRepair" 1
  
  File "${NSISDIR}\\Contrib\\Graphics\\Icons\\modern-uninstall-colorful.ico" 
  WriteRegStr HKLM "Software\Microsoft\Windows\CurrentVersion\Uninstall\OpenALPRMilestone" "DisplayIcon" "$instdir\\modern-uninstall-colorful.ico" 
  WriteUninstaller "uninstall.exe"

SectionEnd

;--------------------------------

; Uninstaller

Section "Uninstall"
  
  ; Remove registry keys
  DeleteRegKey HKLM "Software\Microsoft\Windows\CurrentVersion\Uninstall\OpenALPRMilestone"
  DeleteRegKey HKLM SOFTWARE\OpenALPRMilestone

  ; Stop a service and waits for file release
  SimpleSC::StopService "OpenALPRMilestone" 1 30
  Pop $0 ; returns an errorcode (<>0) otherwise success (0)
  
  SimpleSC::RemoveService "OpenALPRMilestone"

  ; Remove files and uninstaller
  Delete $INSTDIR\Service\OpenALPRQueueMilestone.exe
  Delete $INSTDIR\Service\OpenALPRQueueMilestone.exe.config
  Delete $INSTDIR\Service\Newtonsoft.Json.dll
  Delete $INSTDIR\Service\Newtonsoft.Json.xml
  Delete $INSTDIR\Service\log4net.dll
  Delete $INSTDIR\Service\log4net.xml
  Delete $INSTDIR\Service\Turbocharged.Beanstalk.dll
  Delete $INSTDIR\Service\Turbocharged.Beanstalk.dll.config
  Delete $INSTDIR\Service\Turbocharged.Beanstalk.xml
  Delete $INSTDIR\Service\VideoOS.Platform.dll
  Delete $INSTDIR\Service\VideoOS.Platform.SDK.dll
  Delete $INSTDIR\Service\VideoOS.Platform.Primitives.dll
  Delete $INSTDIR\Service\VideoOS.DatabaseUtility.Common.dll
  Delete $INSTDIR\Service\VideoOS.DatabaseUtility.MediaStorage.dll
  
  Delete $INSTDIR\OpenALPRPlugin.dll
  Delete $INSTDIR\OpenALPRPlugin.dll.config
  Delete $INSTDIR\plugin.def
  
  Delete $INSTDIR\log4net.dll
  Delete $INSTDIR\log4net.xml 
  
  Delete $INSTDIR\uninstall.exe

  ; Remove directories used
  RMDir  /r "$INSTDIR\Service"
  RMDir /r "$INSTDIR"

SectionEnd

