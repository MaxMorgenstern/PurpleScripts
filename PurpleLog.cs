using UnityEngine;
using System;
using System.Linq;
using System.Collections.Generic;
/**
 * 		PurpleLog.Enable ();
 *		PurpleLog.AddListener ("trigger", funktionsname); 
 *
 *		**********
 *
 *		string funktionsname(string[] stringArrayName)
 *		{
 *			for(int i = 0; i < stringArrayName.Length; i++)
 *			{
 *				Debug.Log (stringArrayName[i]);
 *			}
 *			return "rückgabewert";
 *		}
 **/

// DELEGATES FOR CALLBACK
public delegate string PurpleLogCallback(params string[] args); // With message

public class PurpleLog : MonoBehaviour
{
	// PRIVATE/////////////////////////
	private static PurpleLog instance;
	private static string logText;
	private static int logPosition;
	private static Rect consoleRect;

	private static int WINDOW_ID;

	private static string htmlTag;
	private static string colorTag;
	
	private static string colorLog;
	private static string colorError;
	private static string colorWarning;
	private static string colorUser;

	private static string consoleInput;
	private static bool consoleDisplay;
	private static int consoleHistory;
	private static int consoleHistoryCurrent;
	
	private static string toggleKey1;
	private static string toggleKey2;

	private static bool consoleActive;

	private Dictionary<string, PurpleLogCallback> eventListeners;
	

	// OnGUI /////////////////////////
	void OnGUI() {
		if(consoleActive)
		{
			if (consoleDisplay) 
			{
				GUIStyle windowStyle = new GUIStyle (GUI.skin.GetStyle ("window"));
				windowStyle.font = (Font)Resources.Load ("Courier New");
				windowStyle.fontSize = 14;

				GUILayout.Window (WINDOW_ID, consoleRect, Instance.render_window, "Debug Console", windowStyle);
			
			} 
			else if(key_down(toggleKey1) || key_down(toggleKey2))
			{
				open_console();
			}
		}
	}
	
	
	// START UP /////////////////////////
	public PurpleLog ()
	{
		WINDOW_ID = 50;
		eventListeners = new Dictionary<string, PurpleLogCallback>();
		consoleDisplay = false;
		consoleInput = "";
		toggleKey1 = "Caret";
		toggleKey2 = "`";
		consoleHistoryCurrent = 0;
		colorTag = "##colorTag##";
		htmlTag = "<color=##colorType##>##logText##</color>";
		consoleRect = new Rect(0, 0, Screen.width, Mathf.Min(300, Screen.height));

		try{
			colorLog = convert_hex(PurpleConfig.ConsoleLog.Color.Log);
			colorError = convert_hex(PurpleConfig.ConsoleLog.Color.Error);
			colorWarning = convert_hex(PurpleConfig.ConsoleLog.Color.Warning);
			colorUser = convert_hex(PurpleConfig.ConsoleLog.Color.User);
			consoleHistory = PurpleConfig.ConsoleLog.History;
			consoleActive = PurpleConfig.ConsoleLog.Enabled;
		} catch(Exception e){
			colorLog = "'#FFFFFF'";
			colorError = "'#FF6633'";
			colorWarning = "'#FFCC33'";
			colorUser = "'#39E600'";
			consoleHistory = 5;
			consoleActive = false;
			Debug.LogError("Can not read Purple Config! Fallback to default. " + e.ToString());
		}
	}


	// LOG CALLBACK /////////////////////////
	public static void Enable () {
		Application.RegisterLogCallback(Log);
		consoleActive = true;
	}
	
	public static void Disable () {
		Application.RegisterLogCallback(null);
		consoleActive = false;
	}


	// SINGLETON /////////////////////////
	public static PurpleLog Instance
	{
		get
		{
			if (instance == null)
			{
				GameObject gameObject 	= new GameObject ("PurpleLogManager");
				instance     			= gameObject.AddComponent<PurpleLog> ();
			}
			return instance;
		}
	}
	

	// PUBLIC FUNCTIONS /////////////////////////
	public static void Log(string text, string stackTrace, LogType type)
	{
		Instance.log_func (text, stackTrace, type);
	}

	public static void AddListener(string event_name, PurpleLogCallback listener)
	{
		Instance.add_listener(event_name, listener);
	}

	public static string getLogText
	{
		get
		{
			return logText;
		}
	}

	public static int getLogPosition
	{
		get
		{
			return logPosition;
		}
	}


	// PRIVATE FUNCTIONS /////////////////////////
	private void log_func(string text, string stackTrace, LogType type)
	{
		log_func (text, stackTrace, type.ToString ());
	}

	private void log_func(string text, string stackTrace, string type)
	{
		// set color
		string localColorTag = colorTag.Replace ("Tag", type);
		localColorTag = localColorTag.Replace ("##colorLog##", colorLog);
		localColorTag = localColorTag.Replace ("##colorWarning##", colorWarning);
		localColorTag = localColorTag.Replace ("##colorError##", colorError);
		localColorTag = localColorTag.Replace ("##colorUser##", colorUser);

		string logType = type + ": ";
		//logType = logType.PadRight (9);

		string logTypeText = htmlTag.Replace ("##colorType##", localColorTag);
		logTypeText = logTypeText.Replace ("##logText##", logType + text);

		logText += logTypeText + "\n";

		if (consoleHistoryCurrent >= consoleHistory) {
			logText = delete_lines (logText, 1);
		} else {
			consoleHistoryCurrent++;
			logPosition += 25;
		}

	}

	public static string delete_lines(string s, int linesToRemove)
	{
		return s.Split(Environment.NewLine.ToCharArray(), 
						linesToRemove + 1, 
						StringSplitOptions.RemoveEmptyEntries
				).Skip(linesToRemove)
			.FirstOrDefault();
	}

	private void render_window(int id) {
		handle_submit();
		handle_escape();
		
		GUILayout.BeginScrollView(new Vector2(0, getLogPosition), false, true);
		
		GUIStyle labelStyle = new GUIStyle(GUI.skin.GetStyle("label"));
		labelStyle.font = (Font)Resources.Load("Courier New");;
		labelStyle.fontSize = 14;
		
		GUILayout.Label(getLogText, labelStyle);
		GUILayout.EndScrollView();
		
		GUI.SetNextControlName("input");
		consoleInput = GUILayout.TextField(consoleInput);
		
		GUI.FocusControl("input");
	}
	
	private void handle_submit() {
		if (key_down("[enter]") || key_down("return")) {
			if(!String.IsNullOrEmpty(consoleInput))
			{
				log_func (consoleInput, "", "User");

				string[] parameters;
				string[] separators = {" "};
				parameters = consoleInput.Split(separators ,StringSplitOptions.RemoveEmptyEntries);
				if(parameters.Length > 0)
				{
					trigger_listener(parameters[0], parameters);
				}
			}
			consoleInput = "";
		}
	}
	
	private void handle_escape() {
		if (key_down("escape") /*|| key_down(toggleKey1) || key_down(toggleKey2)*/) {
			close_console();
		}
	}

	private void open_console()
	{
		consoleInput = "";
		consoleDisplay = true;
	}

	private void close_console()
	{
		consoleInput = "";
		consoleDisplay = false;
	}


	// EVENT DISPATCH ////////////////////
	private void add_listener(string event_name, PurpleLogCallback listener)
	{
		if (!eventListeners.ContainsKey (event_name))
		{
			eventListeners.Add(event_name, null);
		}
		
		// delegates can be chained using addition
		eventListeners[event_name] += listener;
	}	
	
	private string trigger_listener(string event_name, string[] event_data)
	{
		if (has_event(event_name)) {
			return eventListeners[event_name](event_data);
		} else {
			Debug.LogError("Command not found");
			return "Command not found";
		} 
	}

	private bool has_event(string event_name) {
		return eventListeners.ContainsKey(event_name);
	}

	private bool key_down(string key) {
		return Event.current.Equals(Event.KeyboardEvent(key));
	}


	// HELPER ////////////////////
	private void wake_up() {  }

	private static string convert_hex(string configString)
	{
		return "'"+configString+"'";
	}
}

