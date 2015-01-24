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
		}


		// HANDLER /////////////////////////

		// BASE /////////////////////////
		public static void server_broadcast_handler (string dataObject, NetworkPlayer np)
		{
			Debug.Log ("Broadcast received: " + np.ToString () + " | " + dataObject);
			if(np.ToString() == Constants.SERVER_ID_STRING && Network.isServer) return;
		}
	}
}
