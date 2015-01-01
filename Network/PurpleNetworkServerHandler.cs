using System;
using System.Collections.Generic;
using UnityEngine;

namespace PurpleNetwork
{
	namespace Server
	{
		public class PurpleNetworkServerHandler
		{
			public static void server_broadcast_handler (string dummyObject, NetworkPlayer np){
				Debug.Log ("############");
				Debug.Log (dummyObject);
				Debug.Log (np);
				Debug.Log ("############");
			}
		}
	}
}
