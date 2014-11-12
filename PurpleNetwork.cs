using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using _PurpleSerializer = PurpleSerializer;

/**
 *		using _PurpleMessages = PurpleNetwork.Messages;
 * 		using _PurpleNetwork = PurpleNetwork.PurpleNetwork;
 *
 * 		_PurpleNetwork.ConnectToServer ();
 *		_PurpleNetwork.AddListener<_PurpleMessages.Example> ("listenername", dummydamdam);
 *
 *		**********
 *
 *		_PurpleMessages.Example message = new _PurpleMessages.Example();
 * 		message.test = "Hallo, Welt!";
 *
 * 		_PurpleNetwork.ToServer ("listenername", message);
 * 		_PurpleNetwork.Broadcast("listenername2", message);
 *
 *		**********
 *
 * 		void listenername (object dummyObject)
 *		{
 *		}
 **/

namespace PurpleNetwork
{
	// Purple Network
	public class PurpleNetwork : MonoBehaviour
	{
		// Network Data
		private static string 	networkHost;
		private static int 		networkPort;
		private static String 	networkPassword;
		private static int 		networkPause;

		// Server Restart
		private static int 		serverPlayer;
		private static int 		serverPort;
		private static String 	serverPassword;

		// Singleton
		private static NetworkView   purpleNetworkView;
		private static PurpleNetwork instance;

		// Events
		private Dictionary<string, PurpleNetCallback> eventListeners;

		// Connection Test
		private static ConnectionTesterStatus _connection_test;
		private static ConnectionTesterStatus _connection_test_NAT;

		// Purple Network Events
		public static event PurpleNetworkEvent PurpleServerInitialized;
		public static event PurpleNetworkEvent PurplePlayerDisconnected;
		public static event PurpleNetworkEvent PurplePlayerConnected;

		public static event PurpleNetworkEvent ConnectedToPurpleServer;
		public static event PurpleNetworkEvent DisconnectedFromPurpleServer;
		public static event PurpleNetworkEvent FailedToConnectToPurpleServer;

		public static event PurpleNetworkEvent PurpleNetworkInstantiate;
		public static event PurpleNetworkEvent SerializePurpleNetworkView;

		public static event PurpleNetworkEvent PurpleNetworkError;


		// START UP /////////////////////////
		protected PurpleNetwork ()
		{
			eventListeners = new Dictionary<string, PurpleNetCallback>();
			networkPause = 500;
			try{
				networkHost = PurpleConfig.Network.Host;
				networkPort = PurpleConfig.Network.Port;
				networkPassword = PurpleConfig.Network.Password;
				networkPause = PurpleConfig.Network.Pause;
			} catch(Exception e){
				Debug.LogError("Can not read Purple Config! Set network pause to 500ms. " + e.ToString());
			}
		}


		// SINGLETON /////////////////////////
		public static PurpleNetwork Instance
		{
			get
			{
				if (instance == null)
				{
					GameObject gameObject 	= new GameObject ("PurpleNetworkManager");
					instance     			= gameObject.AddComponent<PurpleNetwork> ();
					purpleNetworkView		= gameObject.AddComponent<NetworkView>   ();
				}
				return instance;
			}
		}


		// VARIABLES ////////////////////////////

		public static bool IsConnected
		{
			get
			{
				return Instance.is_connected();
			}
		}

		public static ConnectionTesterStatus ConnectionTestStatus
		{
			get
			{
				return Instance.test_connection(false);
			}
		}

		public static ConnectionTesterStatus ConnectionTestNATStatus
		{
			get
			{
				return Instance.test_connection_nat(false);
			}
		}


		// SETUP ////////////////////////////
		public static void Setup (string host, int port, string password, int pause)
		{
			Instance.purple_setup (host, port, password, pause);
		}


		// SERVER ////////////////////////////
		public static void LaunchLocalServer (int player, string localPassword, int localPort)
		{
			Instance.launch_server (player, localPassword, localPort);
		}

		public static void StopLocalServer ()
		{
			Instance.stop_server ();
		}

		public static void RestartLocalServer()
		{
			Instance.restart_server ();
		}


		// CLIENT ////////////////////////////
		public static void ConnectToServer ()
		{
			Instance.connect_to_static ();
		}

		public static void ConnectToServer (string hostname, string password, int port)
		{
			Instance.connect_to (hostname, password, port);
		}

		public static void DisconnectFromServer ()
		{
			Instance.disconnect_from ();
		}

		public static void SwitchServer ()
		{
			Instance.switch_static_connecton ();
		}

		public static void SwitchServer (string hostname, string password, int port)
		{
			Instance.switch_connecton (hostname, password, port);
		}



		public static void AddListener<T> (string event_name, PurpleNetCallback listener)
		{
			Instance.add_listener<T> (event_name, listener);
		}



		public static void Broadcast (string event_name, object message)
		{
			Instance.broadcast (event_name, message);
		}
		public static void Broadcast (string event_name, object message, bool forceXML)
		{
			Instance.broadcast (event_name, message, forceXML);
		}


		public static void ToServer (string event_name, object message)
		{
			Instance.to_server (event_name, message);
		}
		public static void ToServer (string event_name, object message, bool forceXML)
		{
			Instance.to_server (event_name, message, forceXML);
		}


		public static void ToPlayer(NetworkPlayer player, string event_name, object message)
		{
			Instance.to_sender(player, event_name, message);
		}
		public static void ToPlayer(NetworkPlayer player, string event_name, object message, bool forceXML)
		{
			Instance.to_sender(player, event_name, message, forceXML);
		}

		public static void ToPlayer(NetworkMessageInfo info, string event_name, object message)
		{
			Instance.to_sender(info.sender, event_name, message);
		}
		public static void ToPlayer(NetworkMessageInfo info, string event_name, object message, bool forceXML)
		{
			Instance.to_sender(info.sender, event_name, message, forceXML);
		}


		public T ConvertToPurpleMessage <T> (string message)
		{
			return _PurpleSerializer.StringToObjectConverter <T> (message);
		}


		public static void ConnectionTestForce()
		{
			Instance.test_connection (true);
		}


		// PRIVATE ////////////////////////////

		// SETUP ////////////////////////////
		private void purple_setup(string host, int port, string password, int pause)
		{
			networkHost = host;
			networkPort = port;
			networkPassword = password;
			networkPause = pause;
		}


		// SERVER ////////////////////////////

		// CONNECTION CALLS
		private void launch_server(int player, string localPassword, int localPort)
		{
			serverPlayer = player;
			serverPassword = localPassword;
			serverPort = localPort;

			Network.InitializeSecurity ();
			Network.incomingPassword = localPassword;

			bool use_nat = !Network.HavePublicAddress();

			Network.InitializeServer (player, localPort, use_nat);
		}

		private void stop_server()
		{
			Network.Disconnect(networkPause);
		}

		private void restart_server()
		{
			stop_server ();
			launch_server(serverPlayer, serverPassword, serverPort);
		}

		// SERVER EVENTS
		private void OnServerInitialized()
		{
			instance.trigger_purple_event(PurpleServerInitialized);
		}

		private void OnPlayerDisconnected(NetworkPlayer player)
		{
			instance.trigger_purple_event(PurplePlayerDisconnected, player);
		}

		private void OnPlayerConnected(NetworkPlayer player)
		{
			instance.trigger_purple_event(PurplePlayerConnected, player);
		}



		// CLIENT ////////////////////////////

		// CONNECTION CALLS
		private void connect_to(string hostname, string password, int port)
		{
			Network.Connect(hostname, port, password);
		}

		private void connect_to_static()
		{
			if(!String.IsNullOrEmpty(networkHost) && !String.IsNullOrEmpty(networkPassword) && networkPort > 0)
			{
				Network.Connect(networkHost, networkPort, networkPassword);
			} else {
				throw new PurpleException ("Can not connect to server, no connection details set!");
			}
		}

		private void disconnect_from()
		{
			Network.Disconnect(networkPause);
		}

		private void switch_connecton(string hostname, string password, int port)
		{
			disconnect_from ();
			connect_to (hostname, password, port);
		}

		private void switch_static_connecton()
		{
			disconnect_from ();
			connect_to_static ();
		}

		// CLIENT EVENTS
		private void OnConnectedToServer()
		{
			instance.trigger_purple_event(ConnectedToPurpleServer);
		}

		// ALSO SERVER EVENT ON DISCONNECT
		private void OnDisconnectedFromServer()
		{
			instance.trigger_purple_event(DisconnectedFromPurpleServer);
		}

		private void OnFailedToConnect(NetworkConnectionError error)
		{
			Debug.Log("Could not connect to server: " + error);
			instance.trigger_purple_event(FailedToConnectToPurpleServer, error);
		}


		// FURTHER EVENTS
		private void OnSerializeNetworkView(BitStream stream, NetworkMessageInfo info)
		{
			instance.trigger_purple_event(SerializePurpleNetworkView, stream);
		}

		private void OnNetworkInstantiate(NetworkMessageInfo info)
		{
			instance.trigger_purple_event(PurpleNetworkInstantiate, info);
		}


		// EVENT DISPATCH ////////////////////
		private void add_listener <T> (string event_name, PurpleNetCallback listener)
		{
			if (!eventListeners.ContainsKey (event_name))
			{
				eventListeners.Add(event_name, null);
			}
			eventListeners[event_name] += listener;
		}



		// SEND DATA ////////////////////

		// SEND TO ALL
		private void broadcast (string event_name, object message)
		{
			broadcast (event_name, message, false);
		}
		private void broadcast (string event_name, object message, bool forceXML)
		{
			string string_message = _PurpleSerializer.ObjectToStringConverter (message, forceXML);
			purpleNetworkView.RPC("receive_purple_network_message", RPCMode.All, event_name, string_message);
		}

		// SEND TO SERVER
		private void to_server (string event_name, object message)
		{
			to_server (event_name, message, false);
		}
		private void to_server (string event_name, object message, bool forceXML)
		{
			string string_message = _PurpleSerializer.ObjectToStringConverter(message, forceXML);
			if (Network.isServer)
			{
				receive_purple_network_message(event_name, string_message, new NetworkMessageInfo());
			}
			else
			{
				purpleNetworkView.RPC("receive_purple_network_message", RPCMode.Server, event_name, string_message);
			}
		}

		// SEND TO SENDER
		private void to_sender(NetworkPlayer player, string event_name, object message)
		{
			to_sender (player, event_name, message, false);
		}
		private void to_sender(NetworkPlayer player, string event_name, object message, bool forceXML)
		{
			string string_message = _PurpleSerializer.ObjectToStringConverter(message, forceXML);
			purpleNetworkView.RPC("receive_purple_network_message", player, event_name, string_message);
		}



		// HELPER ////////////////////
		private void wake_up() {  }

		private bool is_connected()
		{
			if (Network.connections.Length > 0)
				return true;

			if(Network.isServer)
				return true;

			return false;
		}

		private ConnectionTesterStatus test_connection(bool force)
		{
			_connection_test = Network.TestConnection(force);
			return _connection_test;
		}

		private ConnectionTesterStatus test_connection_nat(bool force)
		{
			_connection_test_NAT = Network.TestConnectionNAT(force);
			return _connection_test_NAT;
		}

		private void trigger_purple_event(PurpleNetworkEvent eve)
		{
			trigger_purple_event (eve, null);
		}

		private void trigger_purple_event(PurpleNetworkEvent eve, object passed_object)
		{
			if(eve != null)
				eve(passed_object);
		}



		// RECEIVE ALL DATA ////////////////////
		[RPC]
		void receive_purple_network_message(string event_name, string string_message, NetworkMessageInfo info)
		{
			try{
				eventListeners[event_name](string_message);
			} catch(Exception e){
				Debug.LogWarning("Can not call: eventListeners["+event_name+"]("+string_message+") - " + e.ToString());
				// notify sender that there was an error
				purpleNetworkView.RPC("receive_purple_network_error", info.sender, event_name, string_message);
			}
		}

		[RPC]
		void receive_purple_network_error(string event_name, string string_message, NetworkMessageInfo info)
		{
			Debug.LogWarning ("receive_purple_network_error: can not find called function:" + event_name + " - " + info.sender.ToString());
			instance.trigger_purple_event(PurpleNetworkError, event_name);
		}

	}

}
