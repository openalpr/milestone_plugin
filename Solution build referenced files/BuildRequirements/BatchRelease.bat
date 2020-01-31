SET DEST="C:\Program Files\VideoOS\MIPPlugins\OpenALPR"
SET SRVDEST="C:\Program Files\VideoOS\MIPPlugins\OpenALPR\Service"

IF EXIST "%~dp0..\..\plugin.def" (
	xcopy /y "%~dp0..\..\plugin.def" %DEST%
)

IF EXIST "OpenALPRPlugin.dll" (
	xcopy /y "OpenALPRPlugin.dll" %DEST%
)

IF EXIST "OpenALPRQueueMilestone.exe" (
	xcopy /y "OpenALPRQueueMilestone.exe" %DEST%
)

IF EXIST "OpenALPRQueueConsumer.Chatter.dll" (
	xcopy /y "OpenALPRQueueConsumer.Chatter.dll" %SRVDEST%
)

IF EXIST "OpenALPRQueueMilestone.exe" (
	xcopy /y "OpenALPRQueueMilestone.exe" %SRVDEST%
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