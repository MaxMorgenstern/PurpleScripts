using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Entities.PurpleMessages.User;
using Entities.PurpleNetwork;
using PurpleDatabase.Helper;
using _PurpleMessages = Entities.PurpleMessages;

namespace PurpleNetwork.Server.Handler
{
	public class Base
	{
		private static PurpleCountdown baseHandlerTick;
		
		public static void register_base_handler()
		{
			baseHandlerTick = PurpleCountdown.NewInstance ();
			baseHandlerTick.TriggerEvent += periodically_validate_player;
			baseHandlerTick.Trigger (60, PurpleServer.CurrentConfig.ClientAuthentificationTimeout/4);
			
			PurpleNetwork.AddListener<_PurpleMessages.Server.Message>("server_broadcast",
			                                                         server_broadcast_handler);
			
			PurpleNetwork.AddListener<_PurpleMessages.Server.Message>("player_authenticate",
			                                                         player_authenticate_handler);
			
			PurpleNetwork.PurplePlayerConnected += on_player_connected;
			PurpleNetwork.PurplePlayerDisconnected += on_player_disconnected;
		}
		
		
		
		// HANDLER /////////////////////////
		
		// BASE /////////////////////////
		public static void server_broadcast_handler (string dataObject, NetworkPlayer np)
		{
			Debug.Log ("Broadcast received: " + np.ToString () + " | " + dataObject);
			if(np.ToString() == Constants.SERVER_ID_STRING && Network.isServer) return;
		}
		
		public static void player_authenticate_handler (string dataObject, NetworkPlayer np)
		{
			Debug.Log ("Authentication received: " + np.ToString ());
			if(np.ToString() == Constants.SERVER_ID_STRING && Network.isServer) return;

			Authentication authObject = PurpleSerializer.StringToObjectConverter<Authentication> (dataObject);
			bool validationResult = false;
			string token = string.Empty;

			if(string.IsNullOrEmpty(authObject.playerPassword))
			{
				validationResult = AccountHelper.ValidateAuthentication (authObject.playerName, authObject.playerToken);
				if(validationResult)
					token = AccountHelper.GenerateToken(authObject.playerName, authObject.playerPassword);
			}
			else 
			{
				validationResult = AccountHelper.Login (authObject.playerName, authObject.playerPassword, out token);
			}

			// save 2 spaces if monitoring is allowed otherwise just one for Admin/Mod/GM
			int maxAllowedConnections = PurpleServer.CurrentConfig.ServerMaxClients;
			maxAllowedConnections -= (PurpleServer.CurrentConfig.ServerAllowMonitoring) ? 2 : 1;

			if(validationResult && PurpleServer.UserList.Count <= maxAllowedConnections)
			{
				// TODO: send token to user

			}
			else if(validationResult)
			{
				PurpleNetworkUser playerReference = get_network_player_reference(np);
				if (playerReference.UserType != UserTypes.User)
				{
					// TODO: send token to mod / GM / monitor

				}
			}
			// TODO: send denial
		
		}

		public static PurpleNetworkUser get_network_player_reference(NetworkPlayer np)
		{
			return PurpleServer.UserList.Find (x => x.UserReference == np);
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


		// PERIODICAL EVENTS /////////////////////////
		private static void periodically_validate_player()
		{
			PurpleServer.UserList.Where(x => !x.UserAuthenticated &&
			                            x.UserConnectedTime.AddSeconds(System.Convert.ToDouble(
				PurpleServer.CurrentConfig.ClientAuthentificationTimeout)) < DateTime.Now )
				.ToList().ForEach( x => {
					Debug.Log ("PurpleNetwork.Server.Handler.Base: Disconnect Unauthenticated User "
					           + x.UserReference.ToString());
					_PurpleMessages.Server.Disconnect disconnectMessage = new _PurpleMessages.Server.Disconnect();
					disconnectMessage.status = 2;
					disconnectMessage.message = PurpleI18n.Get("server_disconnect_unauthenticated");
					PurpleNetwork.ToPlayer(x.UserReference, "server_disconnect_unauthenticated", disconnectMessage);
					Network.CloseConnection(x.UserReference, true);
				});
		}
	}
}
