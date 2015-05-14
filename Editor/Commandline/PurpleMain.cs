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
		Debug.Log ("We passed the EntryPoint!");

		Debug.Log("Example: " + PurpleCommandLine.GetArgument("Example"));

		Debug.Log("TestArg: " + PurpleCommandLine.GetArgument("TestArg"));

		Debug.LogWarning ("- - - ### - - -");
    }
}
