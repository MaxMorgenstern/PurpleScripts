using System;
using System.Collections.Generic;
using UnityEngine;
using _PurpleMessages = Entities.PurpleMessages;

namespace PurpleNetwork.Server.Handler
{
	public class Account
	{
		public static void register_account_handler()
		{
			PurpleNetwork.AddListener("client_create_character", client_create_character_handler);
		}


		// HANDLER /////////////////////////

		// ACCOUNT /////////////////////////
		public static void client_create_character_handler (string dataObject, NetworkPlayer np)
		{
			Debug.Log ("Create Character received: " + np.ToString () + " | " + dataObject);
			if(np.ToString() == Constants.SERVER_ID_STRING && Network.isServer) return;
			// TODO: convert data object to account object - purple messages
		}
	}
}
