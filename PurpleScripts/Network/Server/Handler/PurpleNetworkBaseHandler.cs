using System;
using System.Linq;
using UnityEngine;
using Entities.PurpleNetwork;
using PurpleDatabase.Helper;
using _PMBasic = Entities.PurpleMessages;
using _PMClient = Entities.PurpleMessages.User;
using _PMServer = Entities.PurpleMessages.Server;

namespace PurpleNetwork.Server.Handler
{
	public class Base : Shared
	{
		private static PurpleCountdown baseHandlerTick;
		private static PurpleCountdown baseHandlerSanity;

		public static void register_base_handler()
		{
			baseHandlerTick = PurpleCountdown.NewInstance ("BaseHandlerTick");
			baseHandlerTick.TriggerEvent += periodically_validate_player;
			baseHandlerTick.Trigger (60, PurpleServer.CurrentConfig.ClientAuthentificationTimeout/4);

			if(PurpleServer.CurrentConfig.SanityPeriodical > 0)
			{
				int period = PurpleServer.CurrentConfig.SanityPeriodical*60;
				period = (period < 600) ? 600 : period;
				baseHandlerSanity = PurpleCountdown.NewInstance ("BaseHandlerSanity");
				baseHandlerSanity.TriggerEvent += periodically_sanity_check;
				baseHandlerSanity.Trigger (period, period);
			}

			PurpleNetwork.AddListener("server_broadcast", server_broadcast_handler);

			PurpleNetwork.AddListener("client_ping", client_ping_handler);
			PurpleNetwork.AddListener("client_get_version", client_get_version_handler);
			PurpleNetwork.AddListener("client_authenticate", client_authenticate_handler);
			PurpleNetwork.AddListener("client_authenticate_switch", client_authenticate_switch_handler);
			PurpleNetwork.AddListener("client_generate_token", client_generate_token_handler);
			PurpleNetwork.AddListener("client_logout", client_logout_handler);

			PurpleNetwork.PurplePlayerConnected += on_player_connected;
			PurpleNetwork.PurplePlayerDisconnected += on_player_disconnected;

			PurpleNetwork.DisconnectedFromPurpleServer += remove_base_handler;
		}


		// HANDLER /////////////////////////

		// BASE /////////////////////////
		public static void server_broadcast_handler (string dataObject, NetworkPlayer np)
		{
			PurpleDebug.Log("Broadcast received: #" + np.ToString() + " | " + dataObject, 3);
			if(np.ToString() == Constants.SERVER_ID_STRING && Network.isServer) return;
		}

		public static void client_authenticate_handler (string dataObject, NetworkPlayer np)
		{
			PurpleDebug.Log("Authentication received: #" + np.ToString());
			if(np.ToString() == Constants.SERVER_ID_STRING && Network.isServer) return;

			_PMClient.Authentication authObject = PurpleSerializer.StringToObjectConverter<_PMClient.Authentication> (dataObject);
			bool validationResult = false;
			string newToken = string.Empty;

			if(string.IsNullOrEmpty(authObject.ClientPassword))
			{
				validationResult = AccountHelper.ValidateAuthentication (authObject.ClientName, authObject.ClientToken);
				if(validationResult)
					newToken = AccountHelper.GenerateToken(authObject.ClientName, authObject.ClientToken, np);
			}
			else
			{
				validationResult = AccountHelper.Login (authObject.ClientName, authObject.ClientPassword, np, out newToken);
			}

			authObject.validate = validationResult;
			authObject.timestamp = DateTime.Now;

			authObject.ClientPassword = String.Empty;
			authObject.ClientToken = String.Empty;
			authObject.ClientAuthenticated = false;

			if(validationResult && PurpleServer.UserList.Count <= get_max_allowed_player ())
			{
				authObject.ClientToken = newToken;
				authObject.ClientAuthenticated = true;

			}
			else if(validationResult)
			{
				PurpleNetworkUser playerReference = get_network_player_reference(np);
				if (playerReference.UserType != UserTypes.User)
				{
					authObject.ClientToken = newToken;
					authObject.ClientAuthenticated = true;
				}
			}

			AccountHelper.AddLog(get_network_player_reference(np).UserName, "client_authenticate_handler "
								 + authObject.ClientName + " - "+ authObject.ClientAuthenticated);
			PurpleDebug.Log("Authentication result: " + authObject.ClientName + ": " + authObject.ClientAuthenticated);
			PurpleNetwork.ToPlayer(np, "server_authenticate_result", authObject);
		}

		// TODO: test
		public static void client_authenticate_switch_handler (string dataObject, NetworkPlayer np)
		{
			PurpleDebug.Log("Authentication Switch received: #" + np.ToString());
			if(np.ToString() == Constants.SERVER_ID_STRING && Network.isServer) return;

			_PMClient.Authentication authObject = PurpleSerializer.StringToObjectConverter<_PMClient.Authentication> (dataObject);
			string password_or_token = get_token_or_password(authObject);
			bool validationResult = AccountHelper.ValidateServerSwitch(authObject.ClientName, password_or_token, authObject.ServerSwitchToken);

			// TODO: remove database entry - put to invalid

			authObject.validate = validationResult;

			if (validationResult && PurpleServer.UserList.Count <= get_max_allowed_player ())
			{
				authObject.ClientAuthenticated = true;
			}
			else if(validationResult)
			{
				PurpleNetworkUser playerReference = get_network_player_reference(np);
				if (playerReference.UserType != UserTypes.User)
				{
					authObject.ClientAuthenticated = true;
				}
			}

			AccountHelper.AddLog(get_network_player_reference(np).UserName, "client_authenticate_switch_handler "
				+ authObject.ClientName + " - "+ authObject.ClientAuthenticated);
			PurpleDebug.Log("Authentication Switch result: " + authObject.ClientName + ": " + authObject.ClientAuthenticated);
			PurpleNetwork.ToPlayer(np, "server_authenticate_switch_result", authObject);
		}

		// TODO: test and add call to overview
		public static void client_generate_token_handler (string dataObject, NetworkPlayer np)
		{
			PurpleDebug.Log("Token re-generation received: #" + np.ToString());
			if(np.ToString() == Constants.SERVER_ID_STRING && Network.isServer) return;

			_PMClient.Authentication authObject = PurpleSerializer.StringToObjectConverter<_PMClient.Authentication> (dataObject);
			string password_or_token = get_token_or_password(authObject);

			authObject.ClientToken = AccountHelper.GenerateToken(authObject.ClientName, password_or_token, np);
			authObject.timestamp = DateTime.Now;

			AccountHelper.AddLog(get_network_player_reference(np).UserName,
								 "client_generate_token_handler " + authObject.ClientName);
			PurpleNetwork.ToPlayer(np, "server_generate_token_result", authObject);
		}

		public static void client_logout_handler (string dataObject, NetworkPlayer np)
		{
			PurpleDebug.Log("Logout received: #" + np.ToString());
			if(np.ToString() == Constants.SERVER_ID_STRING && Network.isServer) return;

			_PMClient.Authentication authObject = PurpleSerializer.StringToObjectConverter<_PMClient.Authentication> (dataObject);
			string password_or_token = get_token_or_password(authObject);

			if (AccountHelper.Logout(authObject.ClientName, password_or_token))
			{
				authObject.ClientAuthenticated = false;
				authObject.ClientToken = string.Empty;
			}

			AccountHelper.AddLog(get_network_player_reference(np).UserName,
								 "client_logout_handler " + authObject.ClientName);
			PurpleNetwork.ToPlayer (np, "server_logout_result", authObject);

			if(authObject.ClientAuthenticated == false)
				Network.CloseConnection(np, true);
		}

		public static void client_ping_handler (string dataObject, NetworkPlayer np)
		{
			_PMServer.Ping pingObject = PurpleSerializer.StringToObjectConverter<_PMServer.Ping> (dataObject);
			pingObject.bounceTime = DateTime.Now;
			PurpleNetwork.ToPlayer (np, "server_ping", pingObject);
		}

		public static void client_get_version_handler (string dataObject, NetworkPlayer np)
		{
			_PMServer.Version versionObject = new _PMServer.Version ();
			PurpleVersion pv = new PurpleVersion ();
			versionObject.BuildVersion = pv.Version;
			versionObject.ClientVersion = pv.GetClientVersion ();
			versionObject.ServerVersion = pv.GetServerVersion ();

			PurpleNetwork.ToPlayer (np, "server_version_result", versionObject);
		}

		// EVENT /////////////////////////

		public static void on_player_connected(object data, NetworkPlayer np)
		{
			PurpleNetworkUser newUser = new PurpleNetworkUser (np);
			PurpleServer.UserList.Add (newUser);
		}

		public static void on_player_disconnected(object data, NetworkPlayer np)
		{
			PurpleServer.UserList.RemoveAll (x => x.UserReference == np);
		}


		// PRIVATE /////////////////////////

		// PERIODICAL EVENTS /////////////////////////
		private static void periodically_validate_player()
		{
			PurpleServer.UserList.Where(x => !x.UserAuthenticated &&
										x.UserConnectedTime.AddSeconds(System.Convert.ToDouble(
				PurpleServer.CurrentConfig.ClientAuthentificationTimeout)) < DateTime.Now )
				.ToList().ForEach( x => {
					PurpleDebug.Log("PurpleNetwork.Server.Handler.Base: Disconnect Unauthenticated User #"
							   + x.UserReference.ToString());
					_PMServer.Disconnect disconnectMessage = new _PMServer.Disconnect();
					disconnectMessage.status = 2;
					disconnectMessage.message = PurpleI18n.Get("server_disconnect_unauthenticated");
					PurpleNetwork.ToPlayer(x.UserReference, "server_disconnect_unauthenticated", disconnectMessage);
					Network.CloseConnection(x.UserReference, true);
				});
		}

		private static void periodically_sanity_check()
		{
			if(PurpleServer.CurrentConfig.SanityTest)
			{
				PurpleNetworkServerSanityTester.ServerSanityCheck();
			}
		}

		private static int get_max_allowed_player()
		{
			// save 2 spaces if monitoring is allowed otherwise just one for Admin/Mod/GM
			int maxAllowedConnections = PurpleServer.CurrentConfig.ServerMaxClients;
			maxAllowedConnections -= (PurpleServer.CurrentConfig.ServerAllowMonitoring) ? 2 : 1;

			return maxAllowedConnections;
		}


		// DESTROY /////////////////////////

		public static void remove_base_handler(object ob, NetworkPlayer np)
		{
			baseHandlerTick.DestroyInstance ();
			if(PurpleServer.CurrentConfig.SanityPeriodical > 0)
				baseHandlerSanity.DestroyInstance();

			PurpleNetwork.RemoveListener("server_broadcast", server_broadcast_handler);

			PurpleNetwork.RemoveListener("client_ping", client_ping_handler);
			PurpleNetwork.RemoveListener("client_get_version", client_get_version_handler);
			PurpleNetwork.RemoveListener("client_authenticate", client_authenticate_handler);
			PurpleNetwork.RemoveListener("client_generate_token", client_generate_token_handler);
			PurpleNetwork.RemoveListener("client_logout", client_logout_handler);

			PurpleNetwork.PurplePlayerConnected -= on_player_connected;
			PurpleNetwork.PurplePlayerDisconnected -= on_player_disconnected;

			PurpleNetwork.DisconnectedFromPurpleServer -= remove_base_handler;
		}
	}
}
