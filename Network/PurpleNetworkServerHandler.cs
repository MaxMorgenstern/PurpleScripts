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
				// TODO
			}

			public static void RegisterGameListener()
			{
				// TODO
			}

			public static void RegisterMultiListener()
			{
				RegisterAccountListener ();
				RegisterLobbyListener ();
				RegisterGameListener ();
			}

			public static void RegisterLMonitoringListener()
			{
				// TODO
			}



			// PRIVATE FUNCTIONS /////////////////////////

			private static void register_base_handler()
			{
				// SERVER
				PurpleNetwork.AddListener<PurpleMessages.Server.Message>("server_broadcast", 
						Handler.server_broadcast_handler);
			}

			private static void register_account_handler()
			{
				// CLIENT - Account
				PurpleNetwork.AddListener<PurpleMessages.User.CreateAccount>("client_create_account", 
				        Handler.client_create_account_handler);
				PurpleNetwork.AddListener<PurpleMessages.User.Login>("client_login", 
				        Handler.client_login_handler);
			}




			// HANDLER /////////////////////////

			public static void server_broadcast_handler (string dataObject, NetworkPlayer np)
			{
				Debug.Log ("Broadcast sent: " + np.ToString () + " | " + dataObject);
				if(np.ToString() == SERVER_ID && Network.isServer) return;
			}

			public static void client_create_account_handler (string dataObject, NetworkPlayer np)
			{
				if(np.ToString() == SERVER_ID && Network.isServer) return;
				// TODO: convert data object to account object - purple messages
			}

			public static void client_login_handler (string dataObject, NetworkPlayer np)
			{
				if(np.ToString() == SERVER_ID && Network.isServer) return;
				// TODO: convert data object to login object - purple messages
			}
		}
	}
}
