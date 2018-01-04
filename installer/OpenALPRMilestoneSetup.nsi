
;--------------------------------
;Include Modern UI

	!include "MUI2.nsh"
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
  
  ; Set output path to the installation directory.
  SetOutPath $INSTDIR
  
  ; Put file there
  File "OpenALPRPlugin.dll"
  File "OpenALPRQueueMilestone.exe"
  File "OpenALPRPlugin.dll.config"
  File "plugin.def"
  File "OpenALPRQueueMilestone.exe.config"
  File "Turbocharged.Beanstalk.dll"
  File "Turbocharged.Beanstalk.dll.config"
  File "VideoOS.Platform.SDK.dll"
  File "VideoOS.Platform.dll"
  File "Newtonsoft.Json.dll"
  File "Newtonsoft.Json.xml"
  File "VideoOS.Platform.Primitives.dll"
  File "VideoOS.DatabaseUtility.Common.dll"
  File "VideoOS.DatabaseUtility.MediaStorage.dll"
  File "Turbocharged.Beanstalk.xml"
  File "log4net.dll"
  File "log4net.xml"
  
  ; Install a service - ServiceType own process - StartType automatic - NoDependencies - Logon as System Account
  SimpleSC::InstallService "OpenALPRMilestone" "OpenALPR Milestone" "16" "2" "$PROGRAMFILES64\VideoOS\MIPPlugins\OpenALPR\OpenALPRQueueMilestone.exe" "" "" ""
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
  Delete $INSTDIR\OpenALPRPlugin.dll
  Delete $INSTDIR\OpenALPRQueueMilestone.exe
  Delete $INSTDIR\OpenALPRPlugin.dll.config
  Delete $INSTDIR\plugin.def
  Delete $INSTDIR\OpenALPRQueueMilestone.exe.config
  Delete $INSTDIR\Turbocharged.Beanstalk.dll
  Delete $INSTDIR\Turbocharged.Beanstalk.dll.config
  Delete $INSTDIR\VideoOS.Platform.SDK.dll
  Delete $INSTDIR\VideoOS.Platform.dll
  Delete $INSTDIR\Newtonsoft.Json.dll
  Delete $INSTDIR\Newtonsoft.Json.xml
  Delete $INSTDIR\VideoOS.Platform.Primitives.dll
  Delete $INSTDIR\VideoOS.DatabaseUtility.Common.dll
  Delete $INSTDIR\VideoOS.DatabaseUtility.MediaStorage.dll
  Delete $INSTDIR\Turbocharged.Beanstalk.xml
  Delete $INSTDIR\log4net.dll
  Delete $INSTDIR\log4net.xml
  
  Delete $INSTDIR\uninstall.exe

  ; Remove directories used
  RMDir "$INSTDIR"

SectionEnd

