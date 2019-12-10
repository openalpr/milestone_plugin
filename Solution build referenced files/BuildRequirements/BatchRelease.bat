SET DEST="C:\Program Files\VideoOS\MIPPlugins\OpenALPR"
REM SET SRVDEST="C:\Program Files\VideoOS\MIPPlugins\OpenALPR\Service"

IF EXIST "%~dp0..\..\plugin.def" (
	xcopy /y "%~dp0..\..\plugin.def" %DEST%
)

IF EXIST "OpenALPRPlugin.dll" (
	xcopy /y "OpenALPRPlugin.dll" %DEST%
)

IF EXIST "OpenALPRQueueMilestone.exe" (
	xcopy /y "OpenALPRQueueMilestone.exe" %DEST%
)

REM IF EXIST "OpenALPRQueueConsumer.Chatter.dll" (
REM 	xcopy /y "OpenALPRQueueConsumer.Chatter.dll" %SRVDEST%
REM )
REM 
REM IF EXIST "OpenALPRQueueMilestone.exe" (
REM 	xcopy /y "OpenALPRQueueMilestone.exe" %SRVDEST%
REM )
REM 
REM IF EXIST "VideoOS.DatabaseUtility.Common.dll" (
REM 	xcopy /y "VideoOS.DatabaseUtility.Common.dll" %SRVDEST%
REM )
REM 
REM IF EXIST "VideoOS.DatabaseUtility.MediaStorage.dll" (
REM 	xcopy /y "VideoOS.DatabaseUtility.MediaStorage.dll" %SRVDEST%
REM )
REM 
REM IF EXIST "VideoOS.Platform.dll" (
REM 	xcopy /y "VideoOS.Platform.dll" %SRVDEST%
REM )
REM 
REM IF EXIST "VideoOS.Platform.Primitives.dll" (
REM 	xcopy /y "VideoOS.Platform.Primitives.dll" %SRVDEST%
REM )
REM 
REM IF EXIST "VideoOS.Platform.SDK.dll" (
REM 	xcopy /y "VideoOS.Platform.SDK.dll" %SRVDEST%
REM )