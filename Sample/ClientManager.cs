using UnityEngine;
using _Network = PurpleNetwork.PurpleNetwork;
using _Client = PurpleNetwork.Client.PurpleClient;
using _Handler = PurpleNetwork.Client.Handler;

public class ClientManager : MonoBehaviour {

	public string connectionIP = PurpleConfig.Network.Host;
	public string connectionPassword = PurpleConfig.Network.Password;
	public int connectionPort = PurpleConfig.Network.Port;

	void Start()
	{	
		Debug.Log("Client Manager Started!");
		
		PurpleStorage.PurpleStorage.Setup ();
		PurpleI18n.Setup("de-DE");
		PurpleLog.Enable();

		Debug.Log("Load Config");
		_Client.CurrentConfig.Load ("ClientManagerConfig");

		if (!_Client.CurrentConfig.ConfigLoaded) 
		{
			Debug.Log("No data loaded, we set the default and save!");

			_Client.CurrentConfig.ClientName = "administrator";
			_Client.CurrentConfig.ClientPassword = "PurplePassword";
			_Client.CurrentConfig.ServerHost = connectionIP;
			_Client.CurrentConfig.ServerPassword = connectionPassword;
			_Client.CurrentConfig.ServerPort = connectionPort;
			_Client.CurrentConfig.Save ("ClientManagerConfig");
		}

		_Network.AddListener("server_authenticate_result", authenticate_handler);		
	}


	public static void authenticate_handler (string dataObject, NetworkPlayer np)
	{
		Debug.Log (dataObject);
	}

	void OnGUI()
	{
		// LEFT ////////////////////////////

		GUI.Label(new Rect(10, 10, 200, 20), "Status: Disconnected");

		if (GUI.Button(new Rect(50, 50, 200, 50), "Debug")) 
			Debug.Log("Config Data: " + PurpleSerializer.ObjectToStringConverter(_Client.CurrentConfig));
		
		if (GUI.Button(new Rect(50, 110, 200, 50), "Load Config")) 
		{
			Debug.Log("Load Config");
			_Client.CurrentConfig.Load ("ClientManagerConfig");
		}

		if (GUI.Button(new Rect(50, 170, 200, 50), "Delete Config")) 
		{
			Debug.Log("Delete Config");
			_Client.CurrentConfig.Delete ("ClientManagerConfig");
		}


		// RIGHT ////////////////////////////

		GUI.Box (new Rect (385, 50, 230, 175), "Login");
		// GUI.Window (1, new Rect (100, 10, 200, 75), WindowFunc, "yyyyy");

		GUI.Label(new Rect(400, 65, 200, 20), "Username");
		_Client.CurrentConfig.ClientName = 
			GUI.TextField (new Rect (400, 85, 200, 25), _Client.CurrentConfig.ClientName);
		
		GUI.Label(new Rect(400, 115, 200, 20), "Password");
		_Client.CurrentConfig.ClientPassword = 
			GUI.PasswordField (new Rect (400, 135, 200, 25), _Client.CurrentConfig.ClientPassword, '*');

		if (GUI.Button(new Rect(400, 175, 200, 35), "Login")) 
		{
			_Client.CurrentConfig.Save ("ClientManagerConfig");
			_Client.Connect ();
		}

		GUI.Label(new Rect(10, Screen.height - 30, 200, 20), "Press '^' for console window.");
	}

	void WindowFunc(int id)
	{
	}
}

