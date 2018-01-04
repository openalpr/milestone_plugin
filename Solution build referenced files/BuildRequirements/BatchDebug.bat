
SET DEST="C:\Program Files\VideoOS\MIPPlugins\OpenALPR"

xcopy /y "plugin.def" %DEST%
xcopy /y "OpenALPRPlugin.dll" %DEST%
xcopy /y "OpenALPRPlugin.dll.config" %DEST%

xcopy /y "OpenALPRQueueMilestone.exe" %DEST%
xcopy /y "OpenALPRQueueMilestone.exe.config" %DEST%


