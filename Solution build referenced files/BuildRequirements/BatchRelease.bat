SET DEST="C:\Program Files\VideoOS\MIPPlugins\OpenALPR"

IF EXIST "%~dp0..\..\plugin.def" (
	xcopy /y "%~dp0..\..\plugin.def" %DEST%
)

IF EXIST "OpenALPRPlugin.dll" (
	xcopy /y "OpenALPRPlugin.dll" %DEST%
)

IF EXIST "OpenALPRQueueMilestone.exe" (
	xcopy /y "OpenALPRQueueMilestone.exe" %DEST%
)