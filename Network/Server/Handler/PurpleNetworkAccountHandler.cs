using System;
using System.Collections.Generic;
using UnityEngine;

namespace PurpleNetwork.Server.Handler
{
	public class Account
	{
		public static void register_account_handler()
		{
			PurpleNetwork.AddListener<PurpleMessages.User.CreateAccount>("server_create_account", 
			        server_create_account_handler);
			PurpleNetwork.AddListener<PurpleMessages.User.Login>("server_login", 
			        server_login_handler);
		}


		// HANDLER /////////////////////////

		// ACCOUNT /////////////////////////
		public static void server_create_account_handler (string dataObject, NetworkPlayer np)
		{
			Debug.Log ("Create Account received: " + np.ToString () + " | " + dataObject);
			if(np.ToString() == Constants.SERVER_ID_STRING && Network.isServer) return;
			// TODO: convert data object to account object - purple messages
		}

		public static void server_login_handler (string dataObject, NetworkPlayer np)
		{
			Debug.Log ("Login received: " + np.ToString () + " | " + dataObject);
			if(np.ToString() == Constants.SERVER_ID_STRING && Network.isServer) return;
			// TODO: convert data object to login object - purple messages
		}

	}
}
