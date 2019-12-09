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