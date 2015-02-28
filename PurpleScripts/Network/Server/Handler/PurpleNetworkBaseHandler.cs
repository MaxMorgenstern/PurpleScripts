using System;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

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
			
			PurpleNetwork.AddListener<PurpleMessages.Server.Message>("server_broadcast",
			                                                         server_broadcast_handler);
			
			PurpleNetwork.AddListener<PurpleMessages.Server.Message>("player_authenticate",
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
			
			int maxAllowedConnections = PurpleServer.CurrentConfig.ServerMaxClients;
			// save 2 spaces if monitoring is allowed otherwise just one for Admin/Mod/GM
			maxAllowedConnections -= (PurpleServer.CurrentConfig.ServerAllowMonitoring) ? 2 : 1;
			
			PurpleMessages.User.Authentication auth = 
				PurpleSerializer.StringToObjectConverter<PurpleMessages.User.Authentication> (dataObject);
			
			
			// check authentication
			//		token set? - valid?
			//		password valid?
			//			validate token
			// update PurpleServer.UserList...
			// send response
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
			PurpleServer.UserList.Where(x =>
			                            !x.UserAuthenticated &&
			                            x.UserConnectedTime.AddSeconds(System.Convert.ToDouble(
				PurpleServer.CurrentConfig.ClientAuthentificationTimeout)) < DateTime.Now )
				.ToList().ForEach( x =>
				                  {
					Debug.Log ("PurpleNetwork.Server.Handler.Base: Disconnect Unauthenticated User "
					           + x.UserReference.ToString());
					Network.CloseConnection(x.UserReference, true);
				}
				);
		}
	}
}
