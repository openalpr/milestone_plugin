
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
  !define MUI_HEADERIMAGE_BITMAP "C:\Users\Administrator\Documents\Visual Studio 2019\Projects\milestone_plugin\OpenALPRPlugin\Resources\logo_bluegray.png" ; optional
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
	VIAddVersionKey /LANG=${LANG_ENGLISH} "FileDescription" "Integrating OpenALPR with Milestone Smart Client"
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
  
  File "..\build\OpenALPRQueueConsumer.Chatter.dll"
  File "..\build\OpenALPRQueueConsumer.Chatter.dll.config"
  
  File "..\build\log4net.dll"
  File "..\build\log4net.xml"
  
  File "..\build\System.Configuration.Install.dll"
  
  File "..\build\Newtonsoft.Json.dll"
  File "..\build\Newtonsoft.Json.xml"
  
  File "..\build\VideoOS.DatabaseUtility.Common.dll"
  File "..\build\VideoOS.DatabaseUtility.MediaStorage.dll"
  File "..\build\VideoOS.Platform.dll"
  File "..\build\VideoOS.Platform.Primitives.dll"
  File "..\build\VideoOS.Platform.SDK.dll"
  ;File "..\build\VideoOS.Platform.xml"
  ;File "..\build\VideoOS.Platform.SDK.xml"
  File "..\build\Autofac.dll"
  ;File "..\build\Autofac.xml"
  
  SetOutPath "$INSTDIR\Service\ar-SA"
  File "..\build\ar-SA\VideoOS.Platform.resources.dll"
  File "..\build\ar-SA\VideoOS.Platform.SDK.resources.dll"
  
  SetOutPath "$INSTDIR\Service\bg-BG"
  File "..\build\bg-BG\VideoOS.Platform.resources.dll"
  File "..\build\bg-BG\VideoOS.Platform.SDK.resources.dll"
  
  SetOutPath "$INSTDIR\Service\cs-CZ"
  File "..\build\cs-CZ\VideoOS.Platform.resources.dll"
  File "..\build\cs-CZ\VideoOS.Platform.SDK.resources.dll"
  
  SetOutPath "$INSTDIR\Service\da-DK"
  File "..\build\da-DK\VideoOS.Platform.resources.dll"
  File "..\build\da-DK\VideoOS.Platform.SDK.resources.dll"
  
  SetOutPath "$INSTDIR\Service\de-DE"
  File "..\build\de-DE\VideoOS.Platform.resources.dll"
  File "..\build\de-DE\VideoOS.Platform.SDK.resources.dll"
  
  SetOutPath "$INSTDIR\Service\es-ES"
  File "..\build\es-ES\VideoOS.Platform.resources.dll"
  File "..\build\es-ES\VideoOS.Platform.SDK.resources.dll"
  
  SetOutPath "$INSTDIR\Service\et-EE"
  File "..\build\et-EE\VideoOS.Platform.resources.dll"
  File "..\build\et-EE\VideoOS.Platform.SDK.resources.dll"
  
  SetOutPath "$INSTDIR\Service\fa-IR"
  File "..\build\fa-IR\VideoOS.Platform.resources.dll"
  File "..\build\fa-IR\VideoOS.Platform.SDK.resources.dll"
  
  SetOutPath "$INSTDIR\Service\fi-FI"
  File "..\build\fi-FI\VideoOS.Platform.resources.dll"
  File "..\build\fi-FI\VideoOS.Platform.SDK.resources.dll"
  
  SetOutPath "$INSTDIR\Service\fr-FR"
  File "..\build\fr-FR\VideoOS.Platform.resources.dll"
  File "..\build\fr-FR\VideoOS.Platform.SDK.resources.dll"
  
  SetOutPath "$INSTDIR\Service\he-IL"
  File "..\build\he-IL\VideoOS.Platform.resources.dll"
  File "..\build\he-IL\VideoOS.Platform.SDK.resources.dll"
  
  SetOutPath "$INSTDIR\Service\hi-IN"
  File "..\build\hi-IN\VideoOS.Platform.resources.dll"
  File "..\build\hi-IN\VideoOS.Platform.SDK.resources.dll"
  
  SetOutPath "$INSTDIR\Service\hr-HR"
  File "..\build\hr-HR\VideoOS.Platform.resources.dll"
  File "..\build\hr-HR\VideoOS.Platform.SDK.resources.dll"
  
  SetOutPath "$INSTDIR\Service\hu-HU"
  File "..\build\hu-HU\VideoOS.Platform.resources.dll"
  File "..\build\hu-HU\VideoOS.Platform.SDK.resources.dll"
  
  SetOutPath "$INSTDIR\Service\is-IS"
  File "..\build\is-IS\VideoOS.Platform.resources.dll"
  File "..\build\is-IS\VideoOS.Platform.SDK.resources.dll"
  
  SetOutPath "$INSTDIR\Service\it-IT"
  File "..\build\it-IT\VideoOS.Platform.resources.dll"
  File "..\build\it-IT\VideoOS.Platform.SDK.resources.dll"
  
  SetOutPath "$INSTDIR\Service\ja-JP"
  File "..\build\ja-JP\VideoOS.Platform.resources.dll"
  File "..\build\ja-JP\VideoOS.Platform.SDK.resources.dll"
  
  SetOutPath "$INSTDIR\Service\ko-KR"
  File "..\build\ko-KR\VideoOS.Platform.resources.dll"
  File "..\build\ko-KR\VideoOS.Platform.SDK.resources.dll"
  
  SetOutPath "$INSTDIR\Service\nb-NO"
  File "..\build\nb-NO\VideoOS.Platform.resources.dll"
  File "..\build\nb-NO\VideoOS.Platform.SDK.resources.dll"
  
  SetOutPath "$INSTDIR\Service\nl-NL"
  File "..\build\nl-NL\VideoOS.Platform.resources.dll"
  File "..\build\nl-NL\VideoOS.Platform.SDK.resources.dll"
  
  SetOutPath "$INSTDIR\Service\pl-PL"
  File "..\build\pl-PL\VideoOS.Platform.resources.dll"
  File "..\build\pl-PL\VideoOS.Platform.SDK.resources.dll"
  
  SetOutPath "$INSTDIR\Service\pt-BR"
  File "..\build\pt-BR\VideoOS.Platform.resources.dll"
  File "..\build\pt-BR\VideoOS.Platform.SDK.resources.dll"
  
  SetOutPath "$INSTDIR\Service\ru-RU"
  File "..\build\ru-RU\VideoOS.Platform.resources.dll"
  File "..\build\ru-RU\VideoOS.Platform.SDK.resources.dll"
  
  SetOutPath "$INSTDIR\Service\sk-SK"
  File "..\build\sk-SK\VideoOS.Platform.resources.dll"
  File "..\build\sk-SK\VideoOS.Platform.SDK.resources.dll"
  
  SetOutPath "$INSTDIR\Service\sr-Latn-RS"
  File "..\build\sr-Latn-RS\VideoOS.Platform.resources.dll"
  File "..\build\sr-Latn-RS\VideoOS.Platform.SDK.resources.dll"
  
  SetOutPath "$INSTDIR\Service\sv-SE"
  File "..\build\sv-SE\VideoOS.Platform.resources.dll"
  File "..\build\sv-SE\VideoOS.Platform.SDK.resources.dll"
  
  SetOutPath "$INSTDIR\Service\th-TH"
  File "..\build\th-TH\VideoOS.Platform.resources.dll"
  File "..\build\th-TH\VideoOS.Platform.SDK.resources.dll"
  
  SetOutPath "$INSTDIR\Service\tr-TR"
  File "..\build\tr-TR\VideoOS.Platform.resources.dll"
  File "..\build\tr-TR\VideoOS.Platform.SDK.resources.dll"
  
  SetOutPath "$INSTDIR\Service\zh-CN"
  File "..\build\zh-CN\VideoOS.Platform.resources.dll"
  File "..\build\zh-CN\VideoOS.Platform.SDK.resources.dll"
  
  SetOutPath "$INSTDIR\zh-TW"
  File "..\build\zh-TW\VideoOS.Platform.resources.dll"
  File "..\build\zh-TW\VideoOS.Platform.SDK.resources.dll"
  
  ; Set output path to the installation directory (Plug-in).
  SetOutPath $INSTDIR
  
  ; Put file there
  File "..\build\service_access.ps1"
  File "..\build\OpenALPRPlugin.dll"
  File "..\build\OpenALPRPlugin.dll.config"
  
  File "..\build\OpenALPR.SystemTrayApp.exe"
  File "..\build\OpenALPR.SystemTrayApp.exe.config"
  File "..\build\OpenALPR.WpfFormLibrary.dll"
  File "..\build\OpenALPRQueueConsumer.Chatter.dll"
  File "..\build\OpenALPRQueueConsumer.Chatter.dll.config"
  File "..\SystemTrayApp\Resources\shutter32x32BlueIcon.ico"
  File "..\SystemTrayApp\Resources\shutter32x32 - GrayIcon.ico"
  
  File "..\OpenALPRPlugin\plugin.def"
  File "..\build\log4net.dll"
  File "..\build\log4net.xml"
  
  File "..\build\VideoOS.DatabaseUtility.Common.dll"
  File "..\build\VideoOS.DatabaseUtility.MediaStorage.dll"
  File "..\build\VideoOS.Platform.dll"
  File "..\build\VideoOS.Platform.Primitives.dll"
  File "..\build\VideoOS.Platform.SDK.dll"
  ;File "..\build\VideoOS.Platform.xml"
  ;File "..\build\VideoOS.Platform.SDK.xml"
  File "..\build\Autofac.dll"
  ;File "..\build\Autofac.xml"
  
  SetOutPath "$INSTDIR\ar-SA"
  File "..\build\ar-SA\VideoOS.Platform.resources.dll"
  File "..\build\ar-SA\VideoOS.Platform.SDK.resources.dll"
  
  SetOutPath "$INSTDIR\bg-BG"
  File "..\build\bg-BG\VideoOS.Platform.resources.dll"
  File "..\build\bg-BG\VideoOS.Platform.SDK.resources.dll"
  
  SetOutPath "$INSTDIR\cs-CZ"
  File "..\build\cs-CZ\VideoOS.Platform.resources.dll"
  File "..\build\cs-CZ\VideoOS.Platform.SDK.resources.dll"
  
  SetOutPath "$INSTDIR\da-DK"
  File "..\build\da-DK\VideoOS.Platform.resources.dll"
  File "..\build\da-DK\VideoOS.Platform.SDK.resources.dll"
  
  SetOutPath "$INSTDIR\de-DE"
  File "..\build\de-DE\VideoOS.Platform.resources.dll"
  File "..\build\de-DE\VideoOS.Platform.SDK.resources.dll"
  
  SetOutPath "$INSTDIR\es-ES"
  File "..\build\es-ES\VideoOS.Platform.resources.dll"
  File "..\build\es-ES\VideoOS.Platform.SDK.resources.dll"
  
  SetOutPath "$INSTDIR\et-EE"
  File "..\build\et-EE\VideoOS.Platform.resources.dll"
  File "..\build\et-EE\VideoOS.Platform.SDK.resources.dll"
  
  SetOutPath "$INSTDIR\fa-IR"
  File "..\build\fa-IR\VideoOS.Platform.resources.dll"
  File "..\build\fa-IR\VideoOS.Platform.SDK.resources.dll"
  
  SetOutPath "$INSTDIR\fi-FI"
  File "..\build\fi-FI\VideoOS.Platform.resources.dll"
  File "..\build\fi-FI\VideoOS.Platform.SDK.resources.dll"
  
  SetOutPath "$INSTDIR\fr-FR"
  File "..\build\fr-FR\VideoOS.Platform.resources.dll"
  File "..\build\fr-FR\VideoOS.Platform.SDK.resources.dll"
  
  SetOutPath "$INSTDIR\he-IL"
  File "..\build\he-IL\VideoOS.Platform.resources.dll"
  File "..\build\he-IL\VideoOS.Platform.SDK.resources.dll"
  
  SetOutPath "$INSTDIR\hi-IN"
  File "..\build\hi-IN\VideoOS.Platform.resources.dll"
  File "..\build\hi-IN\VideoOS.Platform.SDK.resources.dll"
  
  SetOutPath "$INSTDIR\hr-HR"
  File "..\build\hr-HR\VideoOS.Platform.resources.dll"
  File "..\build\hr-HR\VideoOS.Platform.SDK.resources.dll"
  
  SetOutPath "$INSTDIR\hu-HU"
  File "..\build\hu-HU\VideoOS.Platform.resources.dll"
  File "..\build\hu-HU\VideoOS.Platform.SDK.resources.dll"
  
  SetOutPath "$INSTDIR\is-IS"
  File "..\build\is-IS\VideoOS.Platform.resources.dll"
  File "..\build\is-IS\VideoOS.Platform.SDK.resources.dll"
  
  SetOutPath "$INSTDIR\it-IT"
  File "..\build\it-IT\VideoOS.Platform.resources.dll"
  File "..\build\it-IT\VideoOS.Platform.SDK.resources.dll"
  
  SetOutPath "$INSTDIR\ja-JP"
  File "..\build\ja-JP\VideoOS.Platform.resources.dll"
  File "..\build\ja-JP\VideoOS.Platform.SDK.resources.dll"
  
  SetOutPath "$INSTDIR\ko-KR"
  File "..\build\ko-KR\VideoOS.Platform.resources.dll"
  File "..\build\ko-KR\VideoOS.Platform.SDK.resources.dll"
  
  SetOutPath "$INSTDIR\nb-NO"
  File "..\build\nb-NO\VideoOS.Platform.resources.dll"
  File "..\build\nb-NO\VideoOS.Platform.SDK.resources.dll"
  
  SetOutPath "$INSTDIR\nl-NL"
  File "..\build\nl-NL\VideoOS.Platform.resources.dll"
  File "..\build\nl-NL\VideoOS.Platform.SDK.resources.dll"
  
  SetOutPath "$INSTDIR\pl-PL"
  File "..\build\pl-PL\VideoOS.Platform.resources.dll"
  File "..\build\pl-PL\VideoOS.Platform.SDK.resources.dll"
  
  SetOutPath "$INSTDIR\pt-BR"
  File "..\build\pt-BR\VideoOS.Platform.resources.dll"
  File "..\build\pt-BR\VideoOS.Platform.SDK.resources.dll"
  
  SetOutPath "$INSTDIR\ru-RU"
  File "..\build\ru-RU\VideoOS.Platform.resources.dll"
  File "..\build\ru-RU\VideoOS.Platform.SDK.resources.dll"
  
  SetOutPath "$INSTDIR\sk-SK"
  File "..\build\sk-SK\VideoOS.Platform.resources.dll"
  File "..\build\sk-SK\VideoOS.Platform.SDK.resources.dll"
  
  SetOutPath "$INSTDIR\sr-Latn-RS"
  File "..\build\sr-Latn-RS\VideoOS.Platform.resources.dll"
  File "..\build\sr-Latn-RS\VideoOS.Platform.SDK.resources.dll"
  
  SetOutPath "$INSTDIR\sv-SE"
  File "..\build\sv-SE\VideoOS.Platform.resources.dll"
  File "..\build\sv-SE\VideoOS.Platform.SDK.resources.dll"
  
  SetOutPath "$INSTDIR\th-TH"
  File "..\build\th-TH\VideoOS.Platform.resources.dll"
  File "..\build\th-TH\VideoOS.Platform.SDK.resources.dll"
  
  SetOutPath "$INSTDIR\tr-TR"
  File "..\build\tr-TR\VideoOS.Platform.resources.dll"
  File "..\build\tr-TR\VideoOS.Platform.SDK.resources.dll"
  
  SetOutPath "$INSTDIR\zh-CN"
  File "..\build\zh-CN\VideoOS.Platform.resources.dll"
  File "..\build\zh-CN\VideoOS.Platform.SDK.resources.dll"
  
  SetOutPath "$INSTDIR\zh-TW"
  File "..\build\zh-TW\VideoOS.Platform.resources.dll"
  File "..\build\zh-TW\VideoOS.Platform.SDK.resources.dll"
  
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

Function .onInit
 
	;MessageBox mb_iconstop "Administrator rights required!"
	UserInfo::GetAccountType
	pop $0
	
	${If} $0 != "Admin" ;Require admin rights on NT4+
		MessageBox mb_iconstop "Administrator rights required!"
        ;SetErrorLevel 740 ;ERROR_ELEVATION_REQUIRED
        ;Quit
	${EndIf}

  ReadRegStr $R0 HKLM \
  "Software\Microsoft\Windows\CurrentVersion\Uninstall\OpenALPRMilestone" \
  "UninstallString"
  StrCmp $R0 "" done
 
  MessageBox MB_OKCANCEL|MB_ICONEXCLAMATION \
  "OpenALPRMilestone is already installed. $\n$\nClick `OK` to remove the \
  previous version or `Cancel` to cancel this upgrade." \
  IDOK uninst
  Abort
 
;Run the uninstaller

uninst:
    ClearErrors
    Exec $R0
  done:
FunctionEnd

;--------------------------------
Section "Start MyApp Service"
	Exec '"$INSTDIR\OpenALPR.SystemTrayApp.exe"'
SectionEnd

;--------------------------------
Section "Start Menu Shortcuts"
	SetShellVarContext current
    CreateShortCut "$SMPROGRAMS\OpenALPR.lnk" "$INSTDIR\OpenALPR.SystemTrayApp.exe" "" "$INSTDIR\shutter32x32BlueIcon.ico" 0
SectionEnd

;--------------------------------
Section "Desktop Shortcut"
    SetShellVarContext current
	CreateShortCut "$DESKTOP\OpenALPR.lnk" "$INSTDIR\OpenALPR.SystemTrayApp.exe" "" "$INSTDIR\shutter32x32BlueIcon.ico" 0
SectionEnd

;--------------------------------
Section "Startup Shortcuts"
	SetShellVarContext current
	CreateShortCut "$SMSTARTUP\OpenALPR.lnk" "$INSTDIR\OpenALPR.SystemTrayApp.exe" "" "$INSTDIR\shutter32x32BlueIcon.ico" 0
SectionEnd
;--------------------------------

; Uninstaller
	
Section "Uninstall"
	KillProcWMI::KillProc "OpenALPR.SystemTrayApp.exe"
	Sleep 300
  
  ; Stop a service and waits for file release
  SimpleSC::StopService "OpenALPRMilestone" 1 30
  Pop $0 ; returns an errorcode (<>0) otherwise success (0)
  
  SimpleSC::RemoveService "OpenALPRMilestone"

	;02639d71-0935-35e8-9d1b-9dd1a2a34627
	;nsisDDE::Execute "OpenALPR.SystemTrayApp-{02639d71-0935-35e8-9d1b-9dd1a2a34627}" "[Quit]"
	
  ; Remove registry keys
  DeleteRegKey HKLM "Software\Microsoft\Windows\CurrentVersion\Uninstall\OpenALPRMilestone"
  DeleteRegKey HKLM SOFTWARE\OpenALPRMilestone
  
  ; Remove files and uninstaller
  Delete $INSTDIR\Service\OpenALPRQueueMilestone.exe
  Delete $INSTDIR\Service\OpenALPRQueueMilestone.exe.config
  Delete $INSTDIR\Service\Newtonsoft.Json.dll
  Delete $INSTDIR\Service\Newtonsoft.Json.xml
  Delete $INSTDIR\Service\log4net.dll
  Delete $INSTDIR\Service\log4net.xml
  
  Delete $INSTDIR\Service\VideoOS.DatabaseUtility.Common.dll
  Delete $INSTDIR\Service\VideoOS.DatabaseUtility.MediaStorage.dll
  Delete $INSTDIR\Service\VideoOS.Platform.dll
  Delete $INSTDIR\Service\VideoOS.Platform.Primitives.dll
  Delete $INSTDIR\Service\VideoOS.Platform.SDK.dll
  ;Delete $INSTDIR\Service\VideoOS.Platform.xml
  ;Delete $INSTDIR\Service\VideoOS.Platform.SDK.xml
  
  Delete $INSTDIR\service_access.ps1
  Delete $INSTDIR\OpenALPRPlugin.dll
  Delete $INSTDIR\OpenALPRPlugin.dll.config
  Delete $INSTDIR\plugin.def
  
  Delete $INSTDIR\log4net.dll
  Delete $INSTDIR\log4net.xml 
  
  Delete $INSTDIR\uninstall.exe

  ; Remove directories used
  RMDir  /r "$INSTDIR\Service"
  RMDir /r "$INSTDIR"
  
  SetShellVarContext current
	Delete "$DESKTOP\OpenALPR.lnk"
	Delete "$SMPROGRAMS\OpenALPR.lnk"
	Delete "$SMSTARTUP\OpenALPR.lnk"

SectionEnd

Section "Start MyApp Service"
	Exec '"$INSTDIR\OpenALPR.SystemTrayApp.exe"'
SectionEnd
