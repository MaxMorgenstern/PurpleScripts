using UnityEngine;
using _PMClient = Entities.PurpleMessages.User;
using _PMServer = Entities.PurpleMessages.Server;

namespace PurpleNetwork.Client.Handler
{
	public class Base
	{
		public static void register_base_handler()
		{
			PurpleNetwork.ConnectedToPurpleServer += connected_to_server_handler;

			PurpleNetwork.AddListener("server_broadcast", server_broadcast_handler);

			PurpleNetwork.AddListener("server_authenticate_result", server_authenticate_result_handler);
			PurpleNetwork.AddListener("server_generate_token_result", server_generate_token_result_handler);
			PurpleNetwork.AddListener("server_logout_result", server_logout_result_handler);
			PurpleNetwork.AddListener("server_ping", server_ping_handler);

			PurpleNetwork.DisconnectedFromPurpleServer += remove_base_handler;
		}


		// HANDLER /////////////////////////

		// BASE /////////////////////////
		public static void server_broadcast_handler (string dataObject, NetworkPlayer np)
		{
			Debug.Log ("Broadcast received: " + np.ToString () + " | " + dataObject);
			if(np.ToString() != Constants.SERVER_ID_STRING) return;

			// TODO:
		}

		public static void server_authenticate_result_handler (string dataObject, NetworkPlayer np)
		{
			_PMClient.Authentication authObject = PurpleSerializer.StringToObjectConverter<_PMClient.Authentication> (dataObject);
			//TODO
		}

		public static void server_generate_token_result_handler (string dataObject, NetworkPlayer np)
		{
			_PMClient.Authentication authObject = PurpleSerializer.StringToObjectConverter<_PMClient.Authentication> (dataObject);
			//TODO
		}

		public static void server_logout_result_handler (string dataObject, NetworkPlayer np)
		{
			_PMClient.Authentication authObject = PurpleSerializer.StringToObjectConverter<_PMClient.Authentication> (dataObject);
			//TODO
		}

		public static void server_ping_handler (string dataObject, NetworkPlayer np)
		{
			_PMServer.Ping pingObject = PurpleSerializer.StringToObjectConverter<_PMServer.Ping> (dataObject);
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

