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
		PurpleI18n.Setup ();

		Debug.Log("Load Config");
		_Client.CurrentConfig.Load ("TestScript");


		if (!_Client.CurrentConfig.ConfigLoaded) 
		{
			Debug.Log("No data loaded, set new one and save!");

			_Client.CurrentConfig.ClientName = "administrator";
			_Client.CurrentConfig.ClientPassword = "PurplePassword";
			_Client.CurrentConfig.ServerHost = connectionIP;
			_Client.CurrentConfig.ServerPassword = connectionPassword;
			_Client.CurrentConfig.ServerPort = connectionPort;
			_Client.CurrentConfig.Save ("TestScript");
		}

		_Network.AddListener("server_authenticate_result", authenticate_handler);

		_Client.Connect ();
	}


	public static void authenticate_handler (string dataObject, NetworkPlayer np)
	{
		Debug.Log (dataObject);
	}

	void OnGUI()
	{

	}
}

