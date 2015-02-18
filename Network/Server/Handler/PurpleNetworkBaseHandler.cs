using System;
using System.Collections.Generic;
using UnityEngine;

namespace PurpleNetwork.Server.Handler
{
	public class Base
	{
		public static void register_base_handler()
		{
			PurpleNetwork.AddListener<PurpleMessages.Server.Message>("server_broadcast",
					server_broadcast_handler);

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

		public static void on_player_connected(object data, NetworkPlayer np)
		{
			int allowedConnections = PurpleServer.CurrentConfig.ServerMaxClients;
			allowedConnections -= (PurpleServer.CurrentConfig.ServerAllowMonitoring) ? 1 : 0;

			if(Network.connections.Length >= allowedConnections)
			{
				// TODO - send full notification
				Network.CloseConnection(np, true);
			}
		}

		public static void on_player_disconnected(object data, NetworkPlayer np)
		{
			
		}
	}
}
