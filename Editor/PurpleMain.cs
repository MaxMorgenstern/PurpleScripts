using UnityEngine;
using System.Collections;
using System;
using System.IO;
using System.Linq;

/*
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


::D:\Unity\Editor\Unity.exe -batchmode -quit -nographics -projectPath C:\Users\Maximilian\Downloads\_Unity\Server -logFile C:\Users\Maximilian\Downloads\_Unity\Server\logNew.txt -executeMethod PurpleMain.EntryPoint

pause
*/

class PurpleMain : MonoBehaviour
{
	
	Windows.ConsoleWindow console = new Windows.ConsoleWindow();
	Windows.ConsoleInput input = new Windows.ConsoleInput();

	string strInput;

	public static void EntryPoint ()
	{
		System.IO.StreamWriter standardOutput = new System.IO.StreamWriter(System.Console.OpenStandardOutput());
		standardOutput.AutoFlush = true;
		System.Console.SetOut(standardOutput);

		Debug.Log ("We passed the EntryPoint!");

		Console.WriteLine ("Console EntryPoint ()");
		print ("Hallo!");

		Console.ReadLine ();


	}

    public void Awake()
    {
		Debug.Log ("We passed Awake");

		Debug.Log("Example: " + PurpleCommandLine.GetArgument("Example"));

		Debug.Log("TestArg: " + PurpleCommandLine.GetArgument("TestArg"));

		Debug.LogWarning ("- - - ### - - -");



		DontDestroyOnLoad( gameObject );

		console.Initialize();
		console.SetTitle( "Rust Server" );
		
		input.OnInputText += OnInputText;

		#if UNITY_5_0
		Application.logMessageReceived += HandleLog;
		#else
		Application.RegisterLogCallback(HandleLog);
		#endif
		
		Debug.Log( "Console Started" );
    }

	void OnInputText( string obj )
	{
		//ConsoleSystem.Run( obj, true );
	}

	void HandleLog( string message, string stackTrace, LogType type )
	{
		if ( type == LogType.Warning )		
			System.Console.ForegroundColor = ConsoleColor.Yellow;
		else if ( type == LogType.Error )	
			System.Console.ForegroundColor = ConsoleColor.Red;
		else								
			System.Console.ForegroundColor = ConsoleColor.White;
		
		// We're half way through typing something, so clear this line ..
		if ( Console.CursorLeft != 0 )
			input.ClearLine();
		
		System.Console.WriteLine( message );
		
		// If we were typing something re-add it.
		input.RedrawInputLine();
	}

	void Update()
	{
		input.Update();
	}

	void OnDestroy()
	{
		console.Shutdown();
	}

}
