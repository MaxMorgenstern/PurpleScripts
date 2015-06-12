using UnityEngine;
using _PMClient = Entities.PurpleMessages.User;
using _PMServer = Entities.PurpleMessages.Server;

namespace PurpleNetwork.Client.Handler
{
	public class Base : Shared
	{
		public static event PurpleNetworkClientEvent ServerBroadcast;
		public static event PurpleNetworkClientEvent Authentication;

		public static void register_base_handler()
		{
			PurpleNetwork.ConnectedToPurpleServer += connected_to_server_handler;
			PurpleNetwork.FailedToConnectToPurpleServer += failed_to_connect_to_server_handler;

			PurpleNetwork.AddListener("server_broadcast", server_broadcast_handler);

			PurpleNetwork.AddListener("server_authenticate_result", server_authenticate_result_handler);
			PurpleNetwork.AddListener("server_authenticate_switch_result", server_authenticate_result_handler);
			PurpleNetwork.AddListener("server_generate_token_result", server_generate_token_result_handler);
			PurpleNetwork.AddListener("server_logout_result", server_logout_result_handler);
			PurpleNetwork.AddListener("server_ping", server_ping_handler);
			PurpleNetwork.AddListener("server_switch", server_switch_handler);
			PurpleNetwork.AddListener("server_version_result", server_version_result_handler);

			PurpleNetwork.DisconnectedFromPurpleServer += remove_base_handler;
		}


		// HANDLER /////////////////////////

		// BASE /////////////////////////
		public static void server_broadcast_handler (string dataObject, NetworkPlayer np)
		{
			PurpleDebug.Log("Broadcast received: " + np.ToString() + " | " + dataObject);
			if(np.ToString() != Constants.SERVER_ID_STRING) return;

			trigger_purple_event (ServerBroadcast, dataObject);
		}

		public static void server_authenticate_result_handler (string dataObject, NetworkPlayer np)
		{
			_PMClient.Authentication authObject = PurpleSerializer.StringToObjectConverter<_PMClient.Authentication> (dataObject);
			if (authObject.validate)
			{
				PurpleClient.CurrentConfig.PlayerAuthenticated = authObject.ClientAuthenticated;
				if (!authObject.ClientAuthenticated)
				{
					// authentication okay but too many clients connected
					trigger_purple_event (Authentication, authObject);
					return;
				}
				PurpleClient.CurrentConfig.ClientToken = authObject.ClientToken;
				PurpleClient.CurrentConfig.ClientTokenCreated = authObject.timestamp;
				// authentication okay
				trigger_purple_event (Authentication, authObject);
			} 
			else 
			{
				// incorect data
				trigger_purple_event (Authentication, authObject);
			}
		}

		public static void server_generate_token_result_handler (string dataObject, NetworkPlayer np)
		{
			_PMClient.Authentication authObject = PurpleSerializer.StringToObjectConverter<_PMClient.Authentication> (dataObject);
			if(!string.IsNullOrEmpty (authObject.ClientToken))
			{
				PurpleClient.CurrentConfig.ClientToken = authObject.ClientToken;
				PurpleClient.CurrentConfig.ClientTokenCreated = authObject.timestamp;
			}
		}

		public static void server_logout_result_handler (string dataObject, NetworkPlayer np)
		{
			PurpleDebug.LogWarning ("Logout: " + dataObject);

			_PMClient.Authentication authObject = PurpleSerializer.StringToObjectConverter<_PMClient.Authentication> (dataObject);

			if(!authObject.ClientAuthenticated)
			{
				// logged out - okay
				PurpleClient.CurrentConfig.PlayerAuthenticated = authObject.ClientAuthenticated;
				PurpleClient.CurrentConfig.ClientToken = authObject.ClientToken;
				PurpleClient.CurrentConfig.ClientTokenCreated = System.DateTime.MinValue;
			}
		}

		public static void server_ping_handler (string dataObject, NetworkPlayer np)
		{
			_PMServer.Ping pingObject = PurpleSerializer.StringToObjectConverter<_PMServer.Ping> (dataObject);
			//TODO
			PurpleDebug.Log (PurpleSerializer.ObjectToStringConverter ((pingObject)));
		}

		public static void server_switch_handler (string dataObject, NetworkPlayer np)
		{
			_PMServer.SwitchMessage switchObject = PurpleSerializer.StringToObjectConverter<_PMServer.SwitchMessage> (dataObject);

			if(!string.IsNullOrEmpty (switchObject.SwitchToken))
				PurpleClient.CurrentConfig.ServerSwitchToken = switchObject.SwitchToken;

			if(PurpleClient.CurrentConfig.ServerHost != switchObject.Hostname
				&& PurpleClient.CurrentConfig.ServerPort != switchObject.Port)
			{
				PurpleClient.SwitchServer (switchObject.Hostname, switchObject.Password, switchObject.Port);
			}
			// TODO - check if switch complete - authenticate with token
		}

		public static void server_version_result_handler (string dataObject, NetworkPlayer np)
		{
			_PMServer.Version versionObject = PurpleSerializer.StringToObjectConverter<_PMServer.Version> (dataObject);
			//TODO
			PurpleDebug.Log (PurpleSerializer.ObjectToStringConverter ((versionObject)));
		}


		// EVENT /////////////////////////

		public static void connected_to_server_handler(object ob, NetworkPlayer np)
		{
			// TODO - server switch 
			if(string.IsNullOrEmpty(PurpleClient.CurrentConfig.ServerSwitchToken))
			{
				Calls.Base.Authenticate (PurpleClient.CurrentConfig);
			}
			else
			{
				Calls.Base.AuthenticateSwitch (PurpleClient.CurrentConfig);
			}
			Calls.Base.GetVersion ();
		}

		public static void failed_to_connect_to_server_handler(object ob, NetworkPlayer np)
		{
			PurpleDebug.Log ("failed_to_connect_to_server_handler" + (NetworkConnectionError)ob);
			// TODO:	
			/*
			 	ob = error
				NetworkConnectionError
			*/
		}


		// DESTROY /////////////////////////

		public static void remove_base_handler(object ob, NetworkPlayer np)
		{
			PurpleClient.CurrentConfig.PlayerAuthenticated = false;
			PurpleNetwork.ConnectedToPurpleServer -= connected_to_server_handler;
			PurpleNetwork.FailedToConnectToPurpleServer -= failed_to_connect_to_server_handler;

			PurpleNetwork.RemoveListener("server_broadcast", server_broadcast_handler);

			PurpleNetwork.RemoveListener("server_authenticate_result", server_authenticate_result_handler);
			PurpleNetwork.RemoveListener("server_authenticate_switch_result", server_authenticate_result_handler);
			PurpleNetwork.RemoveListener("server_generate_token_result", server_generate_token_result_handler);
			PurpleNetwork.RemoveListener("server_logout_result", server_logout_result_handler);
			PurpleNetwork.RemoveListener("server_ping", server_ping_handler);
			PurpleNetwork.RemoveListener("server_switch", server_switch_handler);
			PurpleNetwork.RemoveListener("server_version_result", server_version_result_handler);

			PurpleNetwork.DisconnectedFromPurpleServer -= remove_base_handler;
		}

	}

}

