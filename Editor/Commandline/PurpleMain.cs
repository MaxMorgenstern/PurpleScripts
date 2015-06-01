using UnityEngine;
using System.Collections;
using System;
using System.Linq;

/*
@ECHO OFF

::Variable
SET ProjectLocation=%~dp0..\..\..\..
SET UnityLocation="D:\Unity\Editor\Unity.exe"

::Execution
%UnityLocation% -batchmode -projectPath %ProjectLocation% -logFile %ProjectLocation%\log.txt -executeMethod PurpleMain.EntryPoint -quit -PurpleArguments:Example=%1;TestArg=%2

pause
*/

class PurpleMain 
{
    public static void EntryPoint()
    {
        PurpleDebug.Log("We passed the EntryPoint!");

        PurpleDebug.Log("Example: " + PurpleCommandLine.GetArgument("Example"));

        PurpleDebug.Log("TestArg: " + PurpleCommandLine.GetArgument("TestArg"));

        PurpleDebug.LogWarning("- - - ### - - -");
    }
}
