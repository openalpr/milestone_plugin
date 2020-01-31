SET DEST="C:\Program Files\VideoOS\MIPPlugins\OpenALPR"
SET SRVDEST="C:\Program Files\VideoOS\MIPPlugins\OpenALPR\Service"

IF EXIST "%~dp0..\..\plugin.def" (
	xcopy /y "%~dp0..\..\plugin.def" %DEST%
)

IF EXIST "OpenALPRPlugin.dll" (
	xcopy /y "OpenALPRPlugin.dll" %DEST%
)

IF EXIST "OpenALPRPlugin.dll.config" (
	xcopy /y "OpenALPRPlugin.dll.config" %DEST%
)

IF EXIST "OpenALPRQueueMilestone.exe" (
	xcopy /y "OpenALPRQueueMilestone.exe" %DEST%
)

IF EXIST "OpenALPRQueueMilestone.exe.config" (
	xcopy /y "OpenALPRQueueMilestone.exe.config" %DEST%
)

IF EXIST "OpenALPRQueueConsumer.Chatter.dll" (
	xcopy /y "OpenALPRQueueConsumer.Chatter.dll" %SRVDEST%
)

IF EXIST "OpenALPRQueueConsumer.Chatter.dll.config" (
	xcopy /y "OpenALPRQueueConsumer.Chatter.dll.config" %SRVDEST%
)

IF EXIST "OpenALPRQueueConsumer.Chatter.pdb" (
	xcopy /y "OpenALPRQueueConsumer.Chatter.pdb" %SRVDEST%
)

IF EXIST "OpenALPRQueueMilestone.exe" (
	xcopy /y "OpenALPRQueueMilestone.exe" %SRVDEST%
)

IF EXIST "OpenALPRQueueMilestone.exe.config" (
	xcopy /y "OpenALPRQueueMilestone.exe.config" %SRVDEST%
)

IF EXIST "OpenALPRQueueMilestone.pdb" (
	xcopy /y "OpenALPRQueueMilestone.pdb" %SRVDEST%
)

IF EXIST "VideoOS.DatabaseUtility.Common.dll" (
	xcopy /y "VideoOS.DatabaseUtility.Common.dll" %SRVDEST%
)

IF EXIST "VideoOS.DatabaseUtility.MediaStorage.dll" (
	xcopy /y "VideoOS.DatabaseUtility.MediaStorage.dll" %SRVDEST%
)

IF EXIST "VideoOS.Platform.dll" (
	xcopy /y "VideoOS.Platform.dll" %SRVDEST%
)

IF EXIST "VideoOS.Platform.Primitives.dll" (
	xcopy /y "VideoOS.Platform.Primitives.dll" %SRVDEST%
)

IF EXIST "VideoOS.Platform.SDK.dll" (
	xcopy /y "VideoOS.Platform.SDK.dll" %SRVDEST%
)

IF NOT EXIST "ar-SA" ( 
	mkdir ar-SA
)

IF NOT EXIST "bg-BG" ( 
	mkdir bg-BG
)

IF NOT EXIST "cs-CZ" ( 
	mkdir cs-CZ
)

IF NOT EXIST "da-DK" ( 
	mkdir da-DK
)

IF NOT EXIST "de-DE" ( 
	mkdir de-DE
)

IF NOT EXIST "es-ES" ( 
	mkdir es-ES
)

IF NOT EXIST "et-EE" ( 
	mkdir et-EE
)

IF NOT EXIST "fa-IR" ( 
	mkdir fa-IR
)

IF NOT EXIST "fi-FI" ( 
	mkdir fi-FI
)

IF NOT EXIST "fr-FR" ( 
	mkdir fr-FR
)

IF NOT EXIST "he-IL" ( 
	mkdir he-IL
)

IF NOT EXIST "hi-IN" ( 
	mkdir hi-IN
)

IF NOT EXIST "hr-HR" ( 
	mkdir hr-HR
)

IF NOT EXIST "hu-HU" ( 
	mkdir hu-HU
)

IF NOT EXIST "is-IS" ( 
	mkdir is-IS
)

IF NOT EXIST "it-IT" ( 
	mkdir it-IT
)

IF NOT EXIST "ja-JP" ( 
	mkdir ja-JP
)

IF NOT EXIST "ko-KR" ( 
	mkdir ko-KR
)

IF NOT EXIST "nb-NO" ( 
	mkdir nb-NO
)

IF NOT EXIST "nl-NL" ( 
	mkdir nl-NL
)

IF NOT EXIST "pl-PL" ( 
	mkdir pl-PL
)

IF NOT EXIST "pt-BR" ( 
	mkdir pt-BR
)

IF NOT EXIST "ru-RU" ( 
	mkdir ru-RU
)

IF NOT EXIST "sk-SK" ( 
	mkdir sk-SK
)

IF NOT EXIST "sr-Latn-RS" ( 
	mkdir sr-Latn-RS
)

IF NOT EXIST "sv-SE" ( 
	mkdir sv-SE
)

IF NOT EXIST "th-TH" ( 
	mkdir th-TH
)

IF NOT EXIST "tr-TR" ( 
	mkdir tr-TR
)

IF NOT EXIST "zh-CN" ( 
	mkdir zh-CN
)

IF NOT EXIST "zh-TW" ( 
	mkdir zh-TW
)

IF EXIST "..\Solution build referenced files\Milestone\MIPSDK_2019R3\VideoOS.Platform.resources.dll" (
	xcopy /y "..\Solution build referenced files\Milestone\MIPSDK_2019R3\VideoOS.Platform.resources.dll" "ar-SA"
	xcopy /y "..\Solution build referenced files\Milestone\MIPSDK_2019R3\VideoOS.Platform.resources.dll" "bg-BG"
	xcopy /y "..\Solution build referenced files\Milestone\MIPSDK_2019R3\VideoOS.Platform.resources.dll" "cs-CZ"
	xcopy /y "..\Solution build referenced files\Milestone\MIPSDK_2019R3\VideoOS.Platform.resources.dll" "da-DK"
	xcopy /y "..\Solution build referenced files\Milestone\MIPSDK_2019R3\VideoOS.Platform.resources.dll" "de-DE"
	xcopy /y "..\Solution build referenced files\Milestone\MIPSDK_2019R3\VideoOS.Platform.resources.dll" "es-ES"
	xcopy /y "..\Solution build referenced files\Milestone\MIPSDK_2019R3\VideoOS.Platform.resources.dll" "et-EE"
	xcopy /y "..\Solution build referenced files\Milestone\MIPSDK_2019R3\VideoOS.Platform.resources.dll" "fa-IR"
	xcopy /y "..\Solution build referenced files\Milestone\MIPSDK_2019R3\VideoOS.Platform.resources.dll" "fi-FI"
	xcopy /y "..\Solution build referenced files\Milestone\MIPSDK_2019R3\VideoOS.Platform.resources.dll" "fr-FR"
	xcopy /y "..\Solution build referenced files\Milestone\MIPSDK_2019R3\VideoOS.Platform.resources.dll" "he-IL"
	xcopy /y "..\Solution build referenced files\Milestone\MIPSDK_2019R3\VideoOS.Platform.resources.dll" "hi-IN"
	xcopy /y "..\Solution build referenced files\Milestone\MIPSDK_2019R3\VideoOS.Platform.resources.dll" "hr-HR"
	xcopy /y "..\Solution build referenced files\Milestone\MIPSDK_2019R3\VideoOS.Platform.resources.dll" "hu-HU"
	xcopy /y "..\Solution build referenced files\Milestone\MIPSDK_2019R3\VideoOS.Platform.resources.dll" "is-IS"
	xcopy /y "..\Solution build referenced files\Milestone\MIPSDK_2019R3\VideoOS.Platform.resources.dll" "it-IT"
	xcopy /y "..\Solution build referenced files\Milestone\MIPSDK_2019R3\VideoOS.Platform.resources.dll" "ja-JP"
	xcopy /y "..\Solution build referenced files\Milestone\MIPSDK_2019R3\VideoOS.Platform.resources.dll" "ko-KR"
	xcopy /y "..\Solution build referenced files\Milestone\MIPSDK_2019R3\VideoOS.Platform.resources.dll" "nb-NO"
	xcopy /y "..\Solution build referenced files\Milestone\MIPSDK_2019R3\VideoOS.Platform.resources.dll" "nl-NL"
	xcopy /y "..\Solution build referenced files\Milestone\MIPSDK_2019R3\VideoOS.Platform.resources.dll" "pl-PL"
	xcopy /y "..\Solution build referenced files\Milestone\MIPSDK_2019R3\VideoOS.Platform.resources.dll" "pt-BR"
	xcopy /y "..\Solution build referenced files\Milestone\MIPSDK_2019R3\VideoOS.Platform.resources.dll" "ru-RU"
	xcopy /y "..\Solution build referenced files\Milestone\MIPSDK_2019R3\VideoOS.Platform.resources.dll" "sk-SK"
	xcopy /y "..\Solution build referenced files\Milestone\MIPSDK_2019R3\VideoOS.Platform.resources.dll" "sr-Latn-RS"
	xcopy /y "..\Solution build referenced files\Milestone\MIPSDK_2019R3\VideoOS.Platform.resources.dll" "sv-SE"
	xcopy /y "..\Solution build referenced files\Milestone\MIPSDK_2019R3\VideoOS.Platform.resources.dll" "th-TH"
	xcopy /y "..\Solution build referenced files\Milestone\MIPSDK_2019R3\VideoOS.Platform.resources.dll" "tr-TR"
	xcopy /y "..\Solution build referenced files\Milestone\MIPSDK_2019R3\VideoOS.Platform.resources.dll" "zh-CN"
	xcopy /y "..\Solution build referenced files\Milestone\MIPSDK_2019R3\VideoOS.Platform.resources.dll" "zh-TW"
)

IF EXIST "..\Solution build referenced files\Milestone\MIPSDK_2019R3\VideoOS.Platform.SDK.resources.dll" (
	xcopy /y "..\Solution build referenced files\Milestone\MIPSDK_2019R3\VideoOS.Platform.SDK.resources.dll" "ar-SA"
	xcopy /y "..\Solution build referenced files\Milestone\MIPSDK_2019R3\VideoOS.Platform.SDK.resources.dll" "bg-BG"
	xcopy /y "..\Solution build referenced files\Milestone\MIPSDK_2019R3\VideoOS.Platform.SDK.resources.dll" "cs-CZ"
	xcopy /y "..\Solution build referenced files\Milestone\MIPSDK_2019R3\VideoOS.Platform.SDK.resources.dll" "da-DK"
	xcopy /y "..\Solution build referenced files\Milestone\MIPSDK_2019R3\VideoOS.Platform.SDK.resources.dll" "de-DE"
	xcopy /y "..\Solution build referenced files\Milestone\MIPSDK_2019R3\VideoOS.Platform.SDK.resources.dll" "es-ES"
	xcopy /y "..\Solution build referenced files\Milestone\MIPSDK_2019R3\VideoOS.Platform.SDK.resources.dll" "et-EE"
	xcopy /y "..\Solution build referenced files\Milestone\MIPSDK_2019R3\VideoOS.Platform.SDK.resources.dll" "fa-IR"
	xcopy /y "..\Solution build referenced files\Milestone\MIPSDK_2019R3\VideoOS.Platform.SDK.resources.dll" "fi-FI"
	xcopy /y "..\Solution build referenced files\Milestone\MIPSDK_2019R3\VideoOS.Platform.SDK.resources.dll" "fr-FR"
	xcopy /y "..\Solution build referenced files\Milestone\MIPSDK_2019R3\VideoOS.Platform.SDK.resources.dll" "he-IL"
	xcopy /y "..\Solution build referenced files\Milestone\MIPSDK_2019R3\VideoOS.Platform.SDK.resources.dll" "hi-IN"
	xcopy /y "..\Solution build referenced files\Milestone\MIPSDK_2019R3\VideoOS.Platform.SDK.resources.dll" "hr-HR"
	xcopy /y "..\Solution build referenced files\Milestone\MIPSDK_2019R3\VideoOS.Platform.SDK.resources.dll" "hu-HU"
	xcopy /y "..\Solution build referenced files\Milestone\MIPSDK_2019R3\VideoOS.Platform.SDK.resources.dll" "is-IS"
	xcopy /y "..\Solution build referenced files\Milestone\MIPSDK_2019R3\VideoOS.Platform.SDK.resources.dll" "it-IT"
	xcopy /y "..\Solution build referenced files\Milestone\MIPSDK_2019R3\VideoOS.Platform.SDK.resources.dll" "ja-JP"
	xcopy /y "..\Solution build referenced files\Milestone\MIPSDK_2019R3\VideoOS.Platform.SDK.resources.dll" "ko-KR"
	xcopy /y "..\Solution build referenced files\Milestone\MIPSDK_2019R3\VideoOS.Platform.SDK.resources.dll" "nb-NO"
	xcopy /y "..\Solution build referenced files\Milestone\MIPSDK_2019R3\VideoOS.Platform.SDK.resources.dll" "nl-NL"
	xcopy /y "..\Solution build referenced files\Milestone\MIPSDK_2019R3\VideoOS.Platform.SDK.resources.dll" "pl-PL"
	xcopy /y "..\Solution build referenced files\Milestone\MIPSDK_2019R3\VideoOS.Platform.SDK.resources.dll" "pt-BR"
	xcopy /y "..\Solution build referenced files\Milestone\MIPSDK_2019R3\VideoOS.Platform.SDK.resources.dll" "ru-RU"
	xcopy /y "..\Solution build referenced files\Milestone\MIPSDK_2019R3\VideoOS.Platform.SDK.resources.dll" "sk-SK"
	xcopy /y "..\Solution build referenced files\Milestone\MIPSDK_2019R3\VideoOS.Platform.SDK.resources.dll" "sr-Latn-RS"
	xcopy /y "..\Solution build referenced files\Milestone\MIPSDK_2019R3\VideoOS.Platform.SDK.resources.dll" "sv-SE"
	xcopy /y "..\Solution build referenced files\Milestone\MIPSDK_2019R3\VideoOS.Platform.SDK.resources.dll" "th-TH"
	xcopy /y "..\Solution build referenced files\Milestone\MIPSDK_2019R3\VideoOS.Platform.SDK.resources.dll" "tr-TR"
	xcopy /y "..\Solution build referenced files\Milestone\MIPSDK_2019R3\VideoOS.Platform.SDK.resources.dll" "zh-CN"
	xcopy /y "..\Solution build referenced files\Milestone\MIPSDK_2019R3\VideoOS.Platform.SDK.resources.dll" "zh-TW"
)