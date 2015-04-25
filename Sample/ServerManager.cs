using UnityEngine;public class ServerManager : MonoBehaviour {		public string connectionIP = PurpleConfig.Network.Host;	public string connectionPassword = PurpleConfig.Network.Password;	public int connectionPort = PurpleConfig.Network.Port;	public int numberOfPlayers = PurpleConfig.Network.MaxPlayer;	void Start()	{		PurpleI18n.Setup("de-DE");		PurpleLog.Enable();
		Debug.Log("Server Manager Started!");
		PurpleLog.AddListener("start", launch_server_command);
		PurpleLog.AddListener("stop", stop_server_command);
		PurpleLog.AddListener("restart", restart_server_command);
		PurpleLog.AddListener("debug", debug_server_command);	}
	void OnGUI()	{		if (Network.peerType == NetworkPeerType.Disconnected) 		{
			GUI.Label(new Rect(10, 10, 200, 20), "Status: Disconnected");						if (GUI.Button(new Rect(50, 50, 200, 50), "Start Server")) 			{
				launch_server();			}		} else {			if(Network.isServer)			{				GUI.Label (new Rect (10, 10, 200, 20), "Status: Connected - Server");				GUI.Label (new Rect (230, 10, 200, 20), "Connected Clients: " + Network.connections.Length + " / " +numberOfPlayers );				GUI.Label(new Rect(450, 10, 200, 20), "IP: " + Network.player.externalIP + ":" + Network.player.externalPort);
				GUI.Label(new Rect(450, 30, 200, 20), "IP: " + Network.player.ipAddress + ":" + Network.player.port);
				GUI.Label(new Rect(450, 50, 200, 20), "Uptime: " + PurpleNetwork.Server.PurpleServer.ServerUptime());
				if (GUI.Button(new Rect(50, 50, 200, 50), "Stop Server"))					PurpleNetwork.Server.PurpleServer.StopServer(5);								if (GUI.Button(new Rect(50, 110, 200, 50), "Restart Server"))					PurpleNetwork.Server.PurpleServer.RestartServer(5);								if (GUI.Button(new Rect(50, 170, 200, 50), "Debug Data"))
					debug_data();			}
		}
		GUI.Label(new Rect(10, Screen.height - 30, 200, 20), "Press '^' for console window.");	}

	private void debug_data()
	{
		Debug.Log("Max Player: " + PurpleConfig.Network.MaxPlayer);
		Debug.Log("Port: " + PurpleConfig.Network.Port);
		Debug.Log("Password: " + PurpleConfig.Network.Password);
		Debug.Log("Connections: " + Network.connections.Length);
		Debug.Log("UserList: " + PurpleSerializer.ObjectToStringConverter(PurpleNetwork.Server.PurpleServer.UserList));
	}

	private void launch_server()
	{
		Debug.Log("Start Server!");
		Entities.PurpleNetwork.Server.ServerConfig psc = new Entities.PurpleNetwork.Server.ServerConfig();
		psc.ServerMaxClients = numberOfPlayers;
		psc.ServerPassword = connectionPassword;
		psc.ServerPort = connectionPort;
		PurpleNetwork.Server.PurpleServer.LaunchServer(psc);

		Debug.Log(numberOfPlayers + " - - " + connectionPassword + " - - " + connectionPort);
	}


	string launch_server_command(string[] args)
	{
		if (args.Length == 2 && PurpleLog.IsHelpRequired(args[1]))
		{
			Debug.Log("Function to launch the server.");
			return string.Empty;
		}

		launch_server();
		return string.Empty;
	}

	string stop_server_command(string[] args)
	{
		if (args.Length == 2 && PurpleLog.IsHelpRequired(args[1]))
		{
			Debug.Log("Function to stop the server.");
			return string.Empty;
		}

		PurpleNetwork.Server.PurpleServer.StopServer(5);
		return string.Empty;
	}

	string restart_server_command(string[] args)
	{
		if (args.Length == 2 && PurpleLog.IsHelpRequired(args[1]))
		{
			Debug.Log("Function to restart the server.");
			return string.Empty;
		}

		PurpleNetwork.Server.PurpleServer.RestartServer(5);
		return string.Empty;
	}

	string debug_server_command(string[] args)
	{
		debug_data();
		return string.Empty;
	}}