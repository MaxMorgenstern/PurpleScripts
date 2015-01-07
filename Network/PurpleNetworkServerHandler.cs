using System;
using System.Collections.Generic;
using UnityEngine;

namespace PurpleNetwork
{
	namespace Server
	{
		public class Handler
		{
			private const string SERVER_ID = "-1";

			// PUBLIC FUNCTIONS /////////////////////////

			public static void RegisterAccountListener()
			{
				register_base_handler ();
				register_account_handler ();
			}

			public static void RegisterLobbyListener()
			{
				register_base_handler ();
				register_lobby_handler ();
			}

			public static void RegisterGameListener()
			{
				register_base_handler ();
				register_game_handler ();
			}

			public static void RegisterMultiListener()
			{
				RegisterAccountListener ();
				RegisterLobbyListener ();
				RegisterGameListener ();
			}

			public static void RegisterLMonitoringListener()
			{
				register_base_handler ();
				register_monitoring_handler ();
			}



			// PRIVATE FUNCTIONS /////////////////////////

			private static void register_base_handler()
			{
				PurpleNetwork.AddListener<PurpleMessages.Server.Message>("server_broadcast", 
						Handler.server_broadcast_handler);
			}

			private static void register_account_handler()
			{
				PurpleNetwork.AddListener<PurpleMessages.User.CreateAccount>("server_create_account", 
				        Handler.server_create_account_handler);
				PurpleNetwork.AddListener<PurpleMessages.User.Login>("server_login", 
				        Handler.server_login_handler);
			}

			private static void register_lobby_handler()
			{
				// TODO
			}

			private static void register_game_handler()
			{
				// TODO
			}

			private static void register_monitoring_handler()
			{
				// TODO
			}



			// HANDLER /////////////////////////

			// BASE /////////////////////////
			public static void server_broadcast_handler (string dataObject, NetworkPlayer np)
			{
				Debug.Log ("Broadcast received: " + np.ToString () + " | " + dataObject);
				if(np.ToString() == SERVER_ID && Network.isServer) return;
			}

			// ACCOUNT /////////////////////////
			public static void server_create_account_handler (string dataObject, NetworkPlayer np)
			{
				Debug.Log ("Create Account received: " + np.ToString () + " | " + dataObject);
				if(np.ToString() == SERVER_ID && Network.isServer) return;
				// TODO: convert data object to account object - purple messages
			}

			public static void server_login_handler (string dataObject, NetworkPlayer np)
			{
				Debug.Log ("Login received: " + np.ToString () + " | " + dataObject);
				if(np.ToString() == SERVER_ID && Network.isServer) return;
				// TODO: convert data object to login object - purple messages
			}

			// LOBBY /////////////////////////

			// GAME /////////////////////////

			// MONITOR /////////////////////////
			
		}
	}
}
