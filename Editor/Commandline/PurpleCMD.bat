@ECHO OFF

::Variable
SET ProjectLocation=%~dp0..\..\..\..
SET UnityLocation="D:\Unity\Editor\Unity.exe"

::Find Unity
IF [%UnityLocation%] == [] (
   for %%i in (Unity.exe) do SET UnityLocation=%%~$PATH:i
)

::Execution
%UnityLocation% -batchmode -nographics -projectPath %ProjectLocation% -logFile %ProjectLocation%\log.txt -executeMethod PurpleMain.EntryPoint -quit -PurpleArguments:Example=%1;TestArg=%2
::%UnityLocation% -batchmode -nographics -projectPath %ProjectLocation% -logFile %ProjectLocation%\log.txt -executeMethod PurpleMain.EntryPoint -PurpleArguments:Example=%1;TestArg=%2

pause