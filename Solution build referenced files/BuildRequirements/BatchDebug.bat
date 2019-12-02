
SET DEST="C:\Program Files\VideoOS\MIPPlugins\OpenALPR"

xcopy /y "plugin.def" %DEST%

xcopy /y "OpenALPRPlugin.dll" %DEST%
xcopy /y "OpenALPRPlugin.dll.config" %DEST%

xcopy /y "OpenALPRQueueMilestone.exe" %DEST%
xcopy /y "OpenALPRQueueMilestone.exe.config" %DEST%

xcopy /y "OpenALPR.SystemTrayApp.exe" %DEST%
xcopy /y "OpenALPR.WpfFormLibrary.dll" %DEST%
xcopy /y "OpenALPR.SystemTrayApp.exe.config" %DEST%


