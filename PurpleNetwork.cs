using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
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

using _PurpleSerializer = PurpleNetwork.PurpleSerializer;
using _JSON = Newtonsoft.Json.JsonConvert;

namespace PurpleNetwork
{
	public class PurpleNetwork : MonoBehaviour
	{

		//public enum connectionState {disconnected, connected, pending, failed};
		//public enum loginState {loggedIn, loggedOut, failed};
		
		//private string storedData;
		//private connectionState serverConnectionState;
		//private loginState 		serverLoginState;

		//public NetworkViewID lastViewId;
		//private Dictionary<string, Type> callbackTypes = new Dictionary<string, Type>();


		// Network Data
		private static string 	networkHost;
		private static int 		networkPort;
		private static String 	networkPassword;
		private static int 		networkPause;
	
		// TODO: Server Restart
		/*
		private static bool		serverRestart;
		private static int 		serverPlayer;
		private static String 	serverPassword;
		*/

		// Version intormation
		private static int 	_Major;
		private static int 	_Minor;
		private static int 	_Status;
		private static int 	_Revision;

		// Singleton
		private static NetworkView   purpleNetworkView;
		private static PurpleNetwork instance;

		private static bool useJSONMessage;

		private Dictionary<string, PurpleNetCallback> eventListeners = new Dictionary<string, PurpleNetCallback>();


		// START UP /////////////////////////
		protected PurpleNetwork ()
		{
			networkHost = "Max-Laptop.fritz.box";
			networkPort = 25001;
			networkPassword = "testPasswort";
			networkPause = 250;

			useJSONMessage = true;

			_Major = 0;		// Major Builds
			_Minor = 0;		// Minor Builds - Functions added
			_Status = 0;	// 0 for alpha - 1 for beta - 2 for release candidate - 3 for (final) release
			_Revision = 1;	// Bugs fixed - changes made to code
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

		
		// VERSION /////////////////////////
		public static string Version 
		{
			get
			{
				return _Major.ToString() +"."+ _Minor.ToString() +"."+ _Status.ToString() +"."+ _Revision.ToString();
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
			// stop Server
			// start Server
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



// DEV ////////////////////////////
/*

	// TODO: PurpleNetworkQueue - klasse klonen die eine queue nutzt
		
		public enum queueType {broadcast, server, player};
		private Queue <message_queue_item> message_queue = new Queue<message_queue_item> (); //new Queue();


		public static void Broadcast (string event_name, object message, bool useQueue)
		{
			if (useQueue) {
				Instance.to_queue (event_name, message, queueType.broadcast, new NetworkPlayer ());
			} else {
				Broadcast(event_name, message);
			}
		}
		
		public static void ToServer (string event_name, object message, bool useQueue)
		{
			if (useQueue) {
				Instance.to_queue (event_name, message, queueType.server, new NetworkPlayer());
			} else {
				ToServer(event_name, message);
			}
		}
		
		public static void ToPlayer(NetworkPlayer player, string event_name, object message, bool useQueue)
		{
			if (useQueue) {
				Instance.to_queue (event_name, message, queueType.player, player);
			} else {
				ToPlayer(player, event_name, message);
			}
		}

		public static void ToPlayer(NetworkMessageInfo info, string event_name, object message, bool useQueue)
		{
			if (useQueue) {
				Instance.to_queue (event_name, message, queueType.player, info.sender);
			} else {
				ToPlayer(info, event_name, message);
			}
		}



		private void to_queue(string event_name, object message, queueType type, NetworkPlayer player)
		{
			message_queue_item mqi = new message_queue_item (event_name, message, type, player);
			message_queue.Enqueue (mqi);

		}

		public static void ProcessQueue(){ Instance.process_queue(); }

		private void process_queue()
		{
			List<message_queue_item> broadcast_list = new List<message_queue_item> ();
			List<message_queue_item> server_list = new List<message_queue_item> ();

			while (message_queue.Count > 0)
			{
				message_queue_item current_item = message_queue.Dequeue ();

				switch (current_item.type) 
				{
					case queueType.broadcast:
						broadcast_list.Add(current_item);
						break;

					case queueType.server:
						server_list.Add(current_item);
						break;

					case queueType.player:
						// Each seperate as this does only affect specific player
						to_sender(current_item.player, current_item.event_name, current_item.message);
						break;

					default:
						Debug.LogError("invalid queue type detected: " + current_item.type.ToString());
						break;
				}
			} // END WHILE

			// TODO - send broadcast and server list
			// queue_broadcast
			// queue_to_server
		}

		private class message_queue_item
		{
			public string event_name;
			public object message;
			public queueType type;
			public NetworkPlayer player;

			public message_queue_item(string en, object me, queueType ty, NetworkPlayer pl)
			{
				event_name = en;
				message = me;
				type = ty;
				player = pl;
			}
		}

		// SEND QUEUE TO ALL
		private void queue_broadcast (string event_name, object message)
		{
			string string_message = object_to_string_converter(message);
			purpleNetworkView.RPC("receive_purple_network_message_queue", RPCMode.All, event_name, string_message);
		}
		
		// SEND QUEUE TO SERVER
		private void queue_to_server (string event_name, object message)
		{
			string string_message = object_to_string_converter(message);
			if (Network.isServer)
			{
				receive_purple_network_message_queue(event_name, string_message, new NetworkMessageInfo());
			}
			else
			{
				purpleNetworkView.RPC("receive_purple_network_message_queue", RPCMode.Server, event_name, string_message);
			}
		}



		// RECEIVE ALL QUEUE DATA ////////////////////
		[RPC]
		void receive_purple_network_message_queue(string event_name, string xml_message, NetworkMessageInfo info)
		{
			// TODO
			Debug.Log ("receive_purple_network_message_queue(string event_name, string xml_message, NetworkMessageInfo info)");
			Debug.Log (xml_message);
			eventListeners[event_name](xml_message);
		}

*/
// * ////////////////////////////

		
		// SERVER ////////////////////////////
		
		// CONNECTION CALLS
		private void launch_server(int player, string localPassword)
		{
			Network.InitializeSecurity ();
			Network.incomingPassword = localPassword;
			
			bool use_nat = !Network.HavePublicAddress();
			
			Network.InitializeServer (player, networkPort, use_nat);
		}
		
		private void stop_server()
		{
			Network.Disconnect(networkPause);
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

		// TODO: test connection before sending RPC
		private bool is_connected()
		{
			if (Network.connections.Length > 0)
				return true;

			if(Network.isServer)
				return true;

			return false;
		}



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
