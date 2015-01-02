using System;
using System.Collections.Generic;
using UnityEngine;

namespace PurpleNetwork
{
	namespace Server
	{
		public class PurpleNetworkServerHandler
		{
			private const string SERVER_ID = "-1";

			public static void server_broadcast_handler (string dataObject, NetworkPlayer np)
			{
				if(np.ToString() == SERVER_ID && Network.isServer) return;
				Debug.Log ("Faked data send: " + np.ToString () + " -- " + dataObject);
			}

			public static void client_login_handler (string dataObject, NetworkPlayer np)
			{
				if(np.ToString() == SERVER_ID && Network.isServer) return;
				// TODO: convert data object to login object - purple messages
			}
		}
	}
}
