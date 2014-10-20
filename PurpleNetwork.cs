using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using _PurpleSerializer = PurpleNetwork.PurpleSerializer;
using _JSON = Newtonsoft.Json.JsonConvert;

/**
 * 		using _PurpleSerializer = PurpleNetwork.PurpleSerializer;
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
 * 		string string_message = object_to_string_converter(message);
 * 		
 * 		_PurpleNetwork.ToServer ("listenername", string_message);
 * 		_PurpleNetwork.Broadcast("listenername2", string_message);
 * 
 *		**********
 *
 * 		void listenername (object dummyObject)
 *		{
 *		}
 **/


// TODO: test connection before sending RPC
// TODO: network data initial settings

namespace PurpleNetwork
{
	public class PurpleNetwork : MonoBehaviour
	{
		// Network Data
		private static string 	networkHost;
		private static int 		networkPort;
		private static String 	networkPassword;
		private static int 		networkPause;
	
		// Server Restart
		private static int 		serverPlayer;
		private static String 	serverPassword;

		// Singleton
		private static NetworkView   purpleNetworkView;
		private static PurpleNetwork instance;

		// Formating
		private static bool useJSONMessage;

		// Events
		private Dictionary<string, PurpleNetCallback> eventListeners = new Dictionary<string, PurpleNetCallback>();


		// START UP /////////////////////////
		protected PurpleNetwork ()
		{
			// TODO: use other data
			networkHost = "Max-Laptop.fritz.box";
			networkPort = 25001;
			networkPassword = "testPasswort";

			// static data
			networkPause = 250;
			useJSONMessage = true;
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


		// SERVER ////////////////////////////
		public static void LaunchLocalServer (int player, string localPassword)
		{
			Instance.launch_server (player, localPassword);
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

		public static void ConnectToServer (string hostname, string password)
		{
			Instance.connect_to (hostname, password);
		}

		public static void DisconnectFromServer ()
		{
			Instance.disconnect_from ();
		}
		
		public static void SwitchServer ()
		{
			Instance.switch_static_connecton ();
		}
		
		public static void SwitchServer (string hostname, string password)
		{
			Instance.switch_connecton (hostname, password);
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
			return string_to_object_converter <T> (message);
		}


		
		// PRIVATE ////////////////////////////

		
		// SERVER ////////////////////////////
		
		// CONNECTION CALLS
		private void launch_server(int player, string localPassword)
		{
			serverPlayer = player;
			serverPassword = localPassword;

			Network.InitializeSecurity ();
			Network.incomingPassword = localPassword;
			
			bool use_nat = !Network.HavePublicAddress();
			
			Network.InitializeServer (player, networkPort, use_nat);
		}
		
		private void stop_server()
		{
			Network.Disconnect(networkPause);
		}

		private void restart_server()
		{
			stop_server ();
			launch_server(serverPlayer, serverPassword);
		}
		
		// SERVER EVENTS
		void OnServerInitialized()                      {  }
		
		void OnPlayerDisconnected(NetworkPlayer player) {  }
		
		void OnPlayerConnected(NetworkPlayer player)    {  }
		


		// CLIENT ////////////////////////////

		// CONNECTION CALLS
		private void connect_to(string hostname, string password)
		{
			Network.Connect(hostname, networkPort, password);
		}

		private void connect_to_static()
		{
			Network.Connect(networkHost, networkPort, networkPassword);
		}

		private void disconnect_from()
		{
			Network.Disconnect(networkPause);
		}

		private void switch_connecton(string hostname, string password)
		{
			disconnect_from ();
			connect_to (hostname, password);
		}

		private void switch_static_connecton()
		{
			disconnect_from ();
			connect_to_static ();
		}
		
		// CLIENT EVENTS
		private void OnConnectedToServer()      { }

		// ALSO SERVER EVENT ON DISCONNECT
		private void OnDisconnectedFromServer() { }

		private void OnFailedToConnect(NetworkConnectionError error)
		{
			Debug.Log("Could not connect to server: " + error);
		}

	

		// HELPER ////////////////////
		private void wake_up() {  }



		// EVENT DISPATCH ////////////////////
		private void add_listener <T> (string event_name, PurpleNetCallback listener)
		{
			if (!eventListeners.ContainsKey (event_name))
			{
				eventListeners.Add(event_name, null);
			}
			// delegates can be chained using addition
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
			string string_message = object_to_string_converter (message, forceXML);
			purpleNetworkView.RPC("receive_purple_network_message", RPCMode.All, event_name, string_message);
		}
		
		// SEND TO SERVER
		private void to_server (string event_name, object message)
		{
			to_server (event_name, message, false);
		}
		private void to_server (string event_name, object message, bool forceXML)
		{
			string string_message = object_to_string_converter(message, forceXML);
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
			string string_message = object_to_string_converter(message, forceXML);
			purpleNetworkView.RPC("receive_purple_network_message", player, event_name, string_message);
		}



		// HELPER METHODS ////////////////////

		// TODO:
		private bool is_connected()
		{
			if (Network.connections.Length > 0)
				return true;

			if(Network.isServer)
				return true;

			return false;
		}



		// CONVERTER METHODS ////////////////////

		// convert an object into a string
		private string object_to_string_converter(object message)
		{
			return object_to_string_converter(message, false);		
		}

		private string object_to_string_converter(object message, bool forceXML)
		{
			string return_message = null;
			if(useJSONMessage && !forceXML)
			{
				try{
					return_message = _JSON.SerializeObject(message);
				} catch(Exception e){
					Debug.LogWarning("Can not convert object to JSON: " + e.ToString());
					Debug.Log("Set message encoding standard to XML");
					useJSONMessage = false;
				}
			}

			if (String.IsNullOrEmpty (return_message)) 
			{
				return_message = _PurpleSerializer.SerializeObjectXML(message);
			}
			return return_message;
		}

		private T string_to_object_converter <T> (string message)
		{
			try{
				return (T)_JSON.DeserializeObject<T>(message);
			} catch(Exception e){
				Debug.LogWarning("Can not convert message using JSON: " + e.ToString());
			}

			try{
				return (T)_PurpleSerializer.DeserializeObjectXML<T>(message);		
			} catch(Exception e){
				Debug.LogWarning("Can not convert message using XML: " + e.ToString());
			}

			return default (T);
		}



		// RECEIVE ALL DATA ////////////////////

		[RPC]
		void receive_purple_network_message(string event_name, string string_message, NetworkMessageInfo info)
		{
			// TODO:
			Debug.Log ("receive_purple_network_message(string event_name, string xml_message, NetworkMessageInfo info)");
			Debug.Log (string_message);
			try{
				eventListeners[event_name](string_message);
			} catch(Exception e){
				Debug.LogWarning("Can not call: eventListeners["+event_name+"]("+string_message+") - " + e.ToString());
			}
		}

	}

	// DELEGATES FOR CALLBACK
	public delegate void PurpleNetCallback(object converted_object); // With message
	
}
