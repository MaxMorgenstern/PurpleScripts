using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PurpleCommandLine
{
	//Config
	private const string purple_args_prefix = "-PurpleArguments:";
	private const char purple_args_separator = ';';

	public static string GetArgument(string argument)
	{
		Dictionary<string, string> argumentDictionary = get_arguments();
		
		if (argumentDictionary.ContainsKey(argument))
		{
			return argumentDictionary[argument];
		}
		return String.Empty;
	}
	
	public static string GetCommandLine()
	{
		string[] args = get_command_line();
		
		if (args.Length > 0)
		{
			return string.Join(" ", args);
		}
		return String.Empty;
	}


	// PRIVATE /////////////////////////

	private static string[] get_command_line()
	{
		return Environment.GetCommandLineArgs();
	}
	
	private static Dictionary<string,string> get_arguments()
	{
		Dictionary<string, string> argumentDictionary = new Dictionary<string, string>();
		string[] commandLineArguments = get_command_line();
		string[] customArguments;
		string[] customBuffer;
		string customArgsStr = "";
		
		try
		{
			customArgsStr = commandLineArguments.Where(row => row.Contains(purple_args_prefix)).Single();
		}
		catch (Exception e)
		{
			PurpleDebug.LogWarning(e);
			return argumentDictionary;
		}
		
		customArgsStr = customArgsStr.Replace(purple_args_prefix, "");
		customArguments = customArgsStr.Split(purple_args_separator);
		
		foreach (string tmpArg in customArguments)
		{
			customBuffer = tmpArg.Split('=');
			if (customBuffer.Length == 2)
			{
				argumentDictionary.Add(customBuffer[0], customBuffer[1]);
			}
		}
		return argumentDictionary;
	}
}
