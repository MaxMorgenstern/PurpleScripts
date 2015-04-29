using UnityEngine;
using _PMClient = Entities.PurpleMessages.User;
using _PMServer = Entities.PurpleMessages.Server;

namespace PurpleNetwork.Client.Handler
{
	public class Base : Shared
	{
		public static event PurpleNetworkClientEvent ServerBroadcast;

		public static void register_base_handler()
		{
			PurpleNetwork.ConnectedToPurpleServer += connected_to_server_handler;

			PurpleNetwork.AddListener("server_broadcast", server_broadcast_handler);

			PurpleNetwork.AddListener("server_authenticate_result", server_authenticate_result_handler);
			PurpleNetwork.AddListener("server_generate_token_result", server_generate_token_result_handler);
			PurpleNetwork.AddListener("server_logout_result", server_logout_result_handler);
			PurpleNetwork.AddListener("server_ping", server_ping_handler);
			PurpleNetwork.AddListener("server_switch", server_switch_handler);						//TODO: Impver Ca	PurpleNetwork.AddListener("server_version_result", server_version_result_handler);

			PurpleNetwork.DisconnectedFromPurpleServer += remove_base_handler;
		}


		// HANDLER /////////////////////////

		// BASE /////////////////////////
		public static void server_broadcast_handler (string dataObject, NetworkPlayer np)
		{
			Debug.Log ("Broadcast received: " + np.ToString () + " | " + dataObject);
			if(np.ToString() != Constants.SERVER_ID_STRING) return;

			// TODO: more?
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
					// TODO: too many clients connected
					return;
				}
				PurpleClient.CurrentConfig.ClientToken = authObject.ClientToken;
				PurpleClient.CurrentConfig.ClientTokenCreated = authObject.timestamp;

			} 
			else 
			{
				// TODO: incorect data
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
			// TODO: trigger?
		}

		public static void server_logout_result_handler (string dataObject, NetworkPlayer np)
		{
			_PMClient.Authentication authObject = PurpleSerializer.StringToObjectConverter<_PMClient.Authentication> (dataObject);
			if(!authObject.ClientAuthenticated)
			{

				// TODO: logged out - okay
			}
		}

		public static void server_ping_handler (string dataObject, NetworkPlayer np)
		{
			_PMServer.Ping pingObject = PurpleSerializer.StringToObjectConverter<_PMServer.Ping> (dataObject);
			//TODO
		}

		public static void server_switch_handler (string dataObject, NetworkPlayer np)
		{
			_PMServer.SwitchMessage switchObject = PurpleSerializer.StringToObjectConverter<_PMServer.SwitchMessage> (dataObject);
			//TODO
		}

		public static void server_version_result_handler (string dataObject, NetworkPlayer np)
		{
			_PMServer.Version switchObject = PurpleSerializer.StringToObjectConverter<_PMServer.Version> (dataObject);
			//TODO
		}

		// EVENT /////////////////////////

		public static void connected_to_server_handler(object ob, NetworkPlayer np)
		{
			Calls.Base.Authenticate (PurpleClient.CurrentConfig);
		}


		// DESTROY /////////////////////////

		public static void remove_base_handler(object ob, NetworkPlayer np)
		{
			PurpleNetwork.ConnectedToPurpleServer -= connected_to_server_handler;

			PurpleNetwork.AddListener("server_broadcast", server_broadcast_handler);

			PurpleNetwork.AddListener("server_authenticate_result", server_broadcast_handler);
			PurpleNetwork.AddListener("server_generate_token_result", server_broadcast_handler);
			PurpleNetwork.AddListener("server_logout_result", server_broadcast_handler);
			PurpleNetwork.AddListener("server_ping", server_broadcast_handler);

			PurpleNetwork.DisconnectedFromPurpleServer -= remove_base_handler;
		}

	}

}

