@ECHO OFF

::Variable
SET ProjectLocation=%~dp0..\..\..\..
SET UnityLocation="D:\Unity\Editor\Unity.exe"

::Find Unity
IF "%UnityLocation%" EQU "" (
    echo TODO We have to search unity
)

::for %i in (Explorer.exe) do @echo. %~$PATH:i

::Execution
%UnityLocation% -batchmode -projectPath %ProjectLocation% -logFile %ProjectLocation%\log.txt -executeMethod PurpleMain.EntryPoint -quit -PurpleArguments:Example=%1;TestArg=%2

pause